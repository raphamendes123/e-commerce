using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.ApiConfigurations
{
    public interface IAspNetUser
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        string GetUserToken();
        string GetUserRefreshToken();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
        HttpContext GetHttpContext();
    }
}
