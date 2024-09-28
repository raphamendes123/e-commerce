using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.Primitives;

namespace Store.Bff.Checkout.API.Services.gRPC
{
    //middleware
    public class GrpcServiceInterceptor : Interceptor
    {
        private readonly ILogger<GrpcServiceInterceptor> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GrpcServiceInterceptor(ILogger<GrpcServiceInterceptor> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>
            (TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            StringValues token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            Metadata? headers = new Metadata
            {
                {"Authorization", token}
            };

            //adicionando headers novos
            var options = context.Options.WithHeaders(headers);

            //implementando as options novas.
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);


            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}
