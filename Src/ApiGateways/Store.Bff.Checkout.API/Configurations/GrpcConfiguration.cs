using Core; 
using MessageBus;
using Core.Message;
using Store.Bff.Checkout.API.Services.gRPC.Interfaces;
using Store.Bff.Checkout.API.Services.gRPC;
using Store.Bff.Checkout.API.Protos;
using Core.ApiConfigurations;

namespace Configurations
{
    public static class GrpcConfiguration
    {
        public static WebApplicationBuilder AddGrpcConfiguration(this WebApplicationBuilder builder)
        {
            //Interceptor e Delegate mesma coisa

            builder.Services.AddSingleton<GrpcServiceInterceptor>();

            builder.Services.AddScoped<IShopCartGrpcService,ShopCartGrpcService>();

            builder.Services.AddGrpcClient<CartProto.CartProtoClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["ShopCartUrl"]);
            })
                .AddInterceptor<GrpcServiceInterceptor>()
                .AllowSelfSignedCertificate();

            return builder;
        }
         
    }
}
