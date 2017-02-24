using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag.AspNetCore;
namespace WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

   app.UseDefaultFiles();
   app.UseStaticFiles();



   #region CORS - alle Zugriffe erlauben
   // ----------------------------- AppInsights
   // PAKET: install-Package Microsoft.AspNet.Cors
   // Namespace: using Microsoft.AspNet.Cors;
   app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
    );
   #endregion

   #region AppInsights
   // ----------------------------- AppInsights
   app.UseApplicationInsightsRequestTelemetry();
   app.UseApplicationInsightsExceptionTelemetry();
   #endregion

   #region Swagger
   // ----------------------------- Swagger
   // PAKET: install-Package NSwag.AspNetCore
   // Namespace: using NSwag.AspNetCore;
   app.UseSwagger(typeof(Startup).GetTypeInfo().Assembly, new SwaggerOwinSettings
   {
    DefaultPropertyNameHandling = NJsonSchema.PropertyNameHandling.CamelCase,
   });

   app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, new SwaggerUiOwinSettings
   {
    DefaultPropertyNameHandling = NJsonSchema.PropertyNameHandling.CamelCase,
   });
   #endregion
   app.UseMvc();
        }
    }
}
