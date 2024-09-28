using Core.Domain.ResponseResult;
using Microsoft.Extensions.Options;
using Configurations; 
using Front.MVC.Extensions;
using Front.MVC.Models;
using Front.MVC.Services.Abstracts;
using Front.MVC.Services.Interfaces;

namespace Front.MVC.Services.Authentication
{
    public class AuthenticationService : Service, IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(HttpClient httpClient, IOptions<AppSettings> settings, ILogger<AuthenticationService> logger)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthenticationUrl);
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<UserLoginResponseViewModel> Login(UserLoginViewModel viewModel)
        {
            HttpResponseMessage? response = await _httpClient.PostAsync("/api/v1/authentication/login", viewModel.ToObjectJson());

            _logger.LogInformation("******************* LOG Login *******************");
            _logger.LogInformation($"response Login { response.StatusCode }");

            if (!HandleResponseErrors(response))
            {
                return new UserLoginResponseViewModel()
                {
                    ResponseResult = await response.DeserializerObjectResponse<ResponseResult>()
                };
            }

            return await response.DeserializerObjectResponse<UserLoginResponseViewModel>(); 
        }

        public async Task<UserLoginResponseViewModel> Register(UserRegisterViewModel viewModel)
        {
            HttpResponseMessage? response = await _httpClient.PostAsync("/api/v1/authentication/register", viewModel.ToObjectJson());

            _logger.LogInformation("******************* LOG Register *******************");
            _logger.LogInformation($"response Register {response.StatusCode}");

            if (!HandleResponseErrors(response))
            {
                return new UserLoginResponseViewModel()
                {
                    ResponseResult = await response.DeserializerObjectResponse<ResponseResult>()
                };
            }

            return await response.DeserializerObjectResponse<UserLoginResponseViewModel>();
        }

        public async Task<UserLoginResponseViewModel> RefreshToken(string refreshToken)
        {
            HttpResponseMessage? response = await _httpClient.PostAsync("/api/v1/authentication/refresh-token", refreshToken.ToObjectJson());

            if (!HandleResponseErrors(response))
            {
                return new UserLoginResponseViewModel()
                {
                    ResponseResult = await response.DeserializerObjectResponse<ResponseResult>()
                };
            }

            return await response.DeserializerObjectResponse<UserLoginResponseViewModel>();
        }
    }
}
