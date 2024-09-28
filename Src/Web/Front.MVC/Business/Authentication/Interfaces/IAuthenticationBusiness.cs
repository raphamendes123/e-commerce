using Front.MVC.Models;

namespace Front.MVC.Business.Authentication.Interfaces
{
    public interface IAuthenticationBusiness
    {
        Task LogIn(UserLoginResponseViewModel viewModel);
        Task Logout();
        bool TokenExpiried();
        Task<bool> RefreshTokenValid();
    }
}
