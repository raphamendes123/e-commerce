using Front.MVC.Models;

namespace Front.MVC.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserLoginResponseViewModel> Login(UserLoginViewModel viewModel);

        Task<UserLoginResponseViewModel> Register(UserRegisterViewModel viewModel);
        Task<UserLoginResponseViewModel> RefreshToken(string refreshToken);
    }
}
