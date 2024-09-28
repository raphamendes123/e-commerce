using Core.ApiConfigurations;
using Core.Message.Integration;
using Core.Message.Integration.Background;
using Core.Security.Core.Interfaces; 
using MessageBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Store.Authentication.API.Domain.Requests;
using Store.Authentication.API.Services.Interfaces;
using Core.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Store.Authentication.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthenticationController : MainControllerApi
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthService _authService;
        private readonly IMessageBus _bus; 
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<JwtSettings> jwtSettings,
                              IAspNetUser user,
                              IAuthService authService,
                              ILogger<AuthenticationController> logger,
                              IMessageBus bus ) : base(user)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _authService = authService;
            _bus = bus;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserRequest request)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };     

            IdentityResult? result = await _userManager.CreateAsync(user, request.Password); 
            
            if (result.Succeeded)
            {
                //fazer integracao por fila
                ResponseMessage? customerResult = await RegisterCustomer(request);

                if (!customerResult.ValidationResult.IsValid)
                {
                
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(customerResult.ValidationResult);
                }

                await _signInManager.SignInAsync(user, isPersistent: false); 

                return CustomResponse(await _authService.GenerateJwt(email: user.Email));
            }

            foreach (IdentityError error in result.Errors)
            {
                AddError(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserRequest request)
        { 

            if (!ModelState.IsValid) CustomResponse(ModelState);
             
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: true); 

            if (result.Succeeded)
            { 
                return CustomResponse(await _authService.GenerateJwt(request.Email));
            }
            else if (result.IsLockedOut)
            {
                AddError(errorMessage: "User blocked due to error attempts.");
            }
            else
            {
                AddError(errorMessage: "Incorrect username or password."); 
            }

            return CustomResponse();
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AddError("Invalid Refresh Token");
                return CustomResponse();
            }

            var token = await _authService.ValidateRefreshToken(Guid.Parse(refreshToken));

            if (token is null)
            {
                AddError("Expired Refresh Token");
                return CustomResponse();
            }

            return CustomResponse(await _authService.GenerateJwt(token.Username));
        }

        [HttpGet]
        public async Task<ResponseMessage> RegisterCustomer(RegisterUserRequest request)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(request.Email);

            RegisterCustomerIntegrationEvent? customerRegister = new RegisterCustomerIntegrationEvent(id: Guid.Parse(user.Id), name: request.Name, email: request.Email, cpf: request.Cpf);

            try
            {
                return await _bus.RequestAsync<RegisterCustomerIntegrationEvent, ResponseMessage>(customerRegister);
            }
            catch //problema de chamada, já faz a exclusão
            {
                await _userManager.DeleteAsync(user);
                throw;
            } 
        }
 
    }
}
