using Front.MVC.Business.Authentication.Interfaces;
using Front.MVC.Extensions;
using Front.MVC.Services.Interfaces;
using Grpc.Core;
using Polly;
using Polly.CircuitBreaker;
using System.Net;

namespace Front.MVC.Middlewares
{
    public class ExpectionMiddleware
    {
        private readonly RequestDelegate _next;
        private static IAuthenticationBusiness _authenticationBusiness;
        private static IAuthenticationService _authenticationService;

        public ExpectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext,
            IAuthenticationBusiness authenticationBusiness,
            IAuthenticationService authenticationService)
        {
            _authenticationBusiness = authenticationBusiness;
            _authenticationService = authenticationService;
            try
            {
                await _next(httpContext);
            }
            catch (CustomHttpRequestException exception)
            {
                await HandleRequestExceptionAsync(httpContext, exception.StatusCode);
            }
            catch (BrokenCircuitException exception)
            {
                HandleCircuitBreakerRequestExceptionAsync(httpContext, exception);
            }
            catch (RpcException exception)
            {
                //400 bad request => INTERNAL
                //401 unathorized => UNAUTHENTICATED 
                //403 forbideen => PERMISSION_DENIED
                //404 not found => UNIMPLEMENTED

                var statusCode = exception.StatusCode switch
                {
                    StatusCode.Internal => 400,
                    StatusCode.Unauthenticated => 401,
                    StatusCode.PermissionDenied => 403,
                    StatusCode.Unimplemented => 404,
                    _ => 500
                };

                var httpStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode.ToString());

                await HandleRequestExceptionAsync(httpContext, httpStatusCode);
            }
        }

        private void HandleCircuitBreakerRequestExceptionAsync(HttpContext context, BrokenCircuitException exception)
        {
            context.Response.Redirect("system-unavailable");
        }

        private static async Task HandleRequestExceptionAsync(HttpContext context, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == HttpStatusCode.Unauthorized)
            {
                if(_authenticationBusiness.TokenExpiried())
                {
                    if (_authenticationBusiness.RefreshTokenValid().Result)
                    {
                        context.Response.Redirect(context.Request.Path);
                        return;
                    };
                }
                _authenticationBusiness.Logout();
                context.Response.Redirect(location: $"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)httpStatusCode;
        }
    }
}
