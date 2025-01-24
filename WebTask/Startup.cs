using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using System.Globalization;
using System.Reflection;

namespace WebTask
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

           
            //INICIO
            var enUsCulture = new CultureInfo("es-EC");
            var localizationOptions = new RequestLocalizationOptions()
            {
                SupportedCultures = new List<CultureInfo>()
        {
            enUsCulture
        },
                SupportedUICultures = new List<CultureInfo>()
        {
            enUsCulture
        },
                DefaultRequestCulture = new RequestCulture(enUsCulture),
                FallBackToParentCultures = false,
                FallBackToParentUICultures = false,
                RequestCultureProviders = null
            };

            app.UseRequestLocalization(localizationOptions);
            //FINAL

            app.UseExceptionHandler("/Handler/Error");
            app.UseHsts();

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();

            // RUTAS
            ConfigureRoutes(app);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            );
        }

        public void ConfigureRoutes(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                // Authenticacion
                endpoints.MapControllerRoute(
                    name: "Login",
                    pattern: "IniciarSesion",
                    defaults: new { area = "Seguridad", controller = "Cuenta", action = "IniciarSesion" });
                endpoints.MapControllerRoute(
                   name: "Logout",
                   pattern: "Areas/Seguridad/Cuenta/CerrarSession",
                   defaults: new { area = "Seguridad", controller = "Cuenta", action = "CerrarSession" });
                endpoints.MapControllerRoute(
                   name: "Home",
                   pattern: "/",
                   defaults: new { controller = "Home", action = "index" });
            });
        }
    }
}
