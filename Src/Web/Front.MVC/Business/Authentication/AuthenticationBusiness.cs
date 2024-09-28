using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Front.MVC.Models;
using Front.MVC.Business.Authentication.Interfaces;
using Core.ApiConfigurations;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Identity.Data;
using System.ComponentModel;

namespace Front.MVC.Business.Authentication
{
    public class AuthenticationBusiness : IAuthenticationBusiness
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAspNetUser _aspNetUser;
        private readonly Front.MVC.Services.Interfaces.IAuthenticationService _authenticationService;

        public AuthenticationBusiness(
            IHttpContextAccessor contextAccessor, 
            IAspNetUser aspNetUser,
            Front.MVC.Services.Interfaces.IAuthenticationService authenticationService)
        {
            _contextAccessor = contextAccessor;
            _aspNetUser = aspNetUser;
            _authenticationService = authenticationService;
        }

        public async Task LogIn(UserLoginResponseViewModel viewModel)
        {
            JwtSecurityToken? token = GetTokenFormat(viewModel.AccessToken);

            List<Claim> listClaim = new List<Claim>();
            listClaim.Add(item: new Claim("JWT", viewModel.AccessToken));
            listClaim.Add(item: new Claim("RefreshToken", viewModel.RefreshToken));
            listClaim.AddRange(token.Claims);

            ClaimsIdentity? identity = new ClaimsIdentity(claims: listClaim, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties? authenticationProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true,
            };

            await _contextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal: new ClaimsPrincipal(identity),
                authenticationProperties);
        }

        public async Task Logout()
        {
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private static JwtSecurityToken GetTokenFormat(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(jwtToken) as JwtSecurityToken;
        }
        public bool TokenExpiried()
        {
            var jwt = _aspNetUser.GetUserToken();

            if(jwt is null)
                return false;

            var token = GetTokenFormat(jwt);
            
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }
        public async Task<bool> RefreshTokenValid()
        {
             var response = await _authenticationService.RefreshToken(_aspNetUser.GetUserRefreshToken());
        
             if (response.AccessToken != null && response.ResponseResult == null)
             {
                await LogIn(response);
                return true;
             }
             return false;
        }
    }
}
