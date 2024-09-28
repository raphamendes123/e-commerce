using Store.Authentication.API.Domain.Data;
using Store.Authentication.API.Domain.Responses;

namespace Store.Authentication.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginUserResponse> GenerateJwt(string email);
        Task<RefreshToken> ValidateRefreshToken(Guid refreshToken);
    }
}
