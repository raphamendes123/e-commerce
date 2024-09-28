using Microsoft.AspNetCore.DataProtection;

namespace Configurations
{
    public static class MvcConfiguration
    {
        public static WebApplicationBuilder AddMvcConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            
            //Load Balancer mesmo padrao de chave
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem
                (
                    new DirectoryInfo(@"/var/data_protection_keys/")
                )
                .SetApplicationName("Store");

            return builder;
        }

        public static WebApplication UseMvcConfiguration(this WebApplication app)
        {
            app.UseExceptionHandler("/error/500");
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Catalog}/{action=Index}/{id?}");

            return app;
        }
    }
}
