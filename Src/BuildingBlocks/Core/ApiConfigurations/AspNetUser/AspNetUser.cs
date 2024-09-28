using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.ApiConfigurations
{
    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Name => _httpContextAccessor.HttpContext.User.Identity.Name;
        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _httpContextAccessor.HttpContext.User.GetUserEmail() : "";
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _httpContextAccessor.HttpContext.User.Claims;
        }


        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext.User.IsInRole(role);
        }

        public string GetUserToken()
        {
            return IsAuthenticated() ? _httpContextAccessor.HttpContext.User.GetUserToken() : string.Empty;
        }

        public HttpContext GetHttpContext()
        {
            return _httpContextAccessor.HttpContext;
        }
        public string GetUserRefreshToken()
        {
            return IsAuthenticated() ? _httpContextAccessor.HttpContext.User.GetUserRefreshToken() : string.Empty;
        }
    }
}
