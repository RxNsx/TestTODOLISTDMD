using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using TODOLISTTRY.DAL;
using TODOLISTTRY.DAL.Interfaces;
using TODOLISTTRY.Services.Interfaces;
using TODOLISTTRY.Services.Services;
using TODOLISTTRY.Web.Infrastructure.ExceptionHandlers;
using TODOLISTTRY.Web.Services;

namespace TODOLISTTRY.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region DbConnection

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.CommandTimeout(60))
                );

            #endregion

            #region Dependency Injection

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDoService, DoService>();
            services.AddSingleton<ErrorMessageTranslationService>();

            #endregion

            #region Localization

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("En"),
                    new CultureInfo("Ru")
                };

                options.DefaultRequestCulture = new RequestCulture("Ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion

            services.AddMvc()
                    .AddDataAnnotationsLocalization(options => {
                        options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                        })
                    .AddViewLocalization()
                    .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRequestLocalization();

            app.UseMiddleware<ExceptionMiddleware>();

            DataSeed.EnsurePopulated(app);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Do}/{action=Index}/{id?}");
            });


        }
    }
}
