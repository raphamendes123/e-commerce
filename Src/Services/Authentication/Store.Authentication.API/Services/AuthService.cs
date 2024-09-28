using Store.Authentication.API.Domain.Responses;
using Store.Authentication.API.Services.Interfaces;
using Core.ApiConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Security.Core.Interfaces;
using Core.Jwt;
using Store.Authentication.API.Domain.Data;
using Polly;
using Store.Authentication.API.Domain.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Store.Authentication.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly JwksSettings _jwksSettings;
        private readonly IAspNetUser _aspNetUser;     
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;


        public AuthService(SignInManager<IdentityUser> signInManager,
                        UserManager<IdentityUser> userManager,
                        IOptions<JwtSettings> jwtSettings,
                        IOptions<JwksSettings> jwksSettings,
                        IAspNetUser aspNetUser,
                        IJwtService jwtService,
                        ApplicationDbContext context,
                        ILogger<AuthService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _jwksSettings = jwksSettings.Value;
            _aspNetUser = aspNetUser;
            _jwtService = jwtService;
            _context = context;
            _logger = logger;
        }

        public async Task<LoginUserResponse> GenerateJwt(string email)
        { 
            IdentityUser? user = await _userManager.FindByEmailAsync(email);
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            ClaimsIdentity claimsIdentity = await GetClaimsIdentity(claims, user);

            string token = await JWKSGenerateEncodedTokenAsync(claimsIdentity);
             
            var refreshToken = await GenerateRefreshToken(email); 
          
            return GetTokenResponse(user, claims, token, refreshToken);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(IList<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            AdicionarRolesComoClaims(claims, userRoles);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;


            static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

            static void AdicionarRolesComoClaims(IList<Claim> claims, IList<string> roles)
            {
                foreach (string userRole in roles)
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
        }

        private string GenerateEncodedToken(ClaimsIdentity claimsIdentity)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            SecurityToken token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
        private async Task<string> JWKSGenerateEncodedTokenAsync(ClaimsIdentity claimsIdentity)
        {
            var currentIssuer = $"{_aspNetUser.GetHttpContext().Request.Scheme}//{_aspNetUser.GetHttpContext().Request.Host}";

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var key = await _jwtService.GetCurrentSigningCredentials();

            SecurityToken token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwksSettings.Issuer,
                Audience = _jwksSettings.Audience,
                Subject = claimsIdentity, 
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = key
            });

            return tokenHandler.WriteToken(token);
        }
        private static LoginUserResponse GetTokenResponse(IdentityUser user, IList<Claim> claims, string encodedToken, RefreshToken refreshToken)
        {
            return new LoginUserResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim
                    {
                        Type = c.Type,
                        Value = c.Value
                    })
                },
                RefreshToken = refreshToken?.Token
            };
        }

        private async Task<RefreshToken> GenerateRefreshToken(string email)
        {
            int expirationHours = (_jwksSettings?.RefreshTokenExpiration  != null ? _jwksSettings?.RefreshTokenExpiration : _jwtSettings?.RefreshTokenExpiration).Value;
           
            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(x => x.Username == email));
            
            RefreshToken? refreshToken = new RefreshToken()
            {
                Username = email,
                ExpirationDate = DateTime.UtcNow.AddHours(expirationHours),
            };

            _context.RefreshTokens.Add(refreshToken);
            
            _context.SaveChanges();
            
            return refreshToken;
        }

        public async Task<RefreshToken> ValidateRefreshToken(Guid refreshToken)
        { 
            var token = await _context.RefreshTokens.AsNoTracking().
                FirstOrDefaultAsync(x => x.Token == refreshToken);

            return token != null && token.ExpirationDate.ToLocalTime() > DateTime.UtcNow ? token : null;
        }
    }
}
