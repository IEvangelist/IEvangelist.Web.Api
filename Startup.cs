using IEvangelist.Web.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace IEvangelist.Web.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
            => Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddRouting(options => options.LowercaseUrls = true)
                    .AddSwaggerGen(config =>
                    {
                        config.SwaggerDoc("v1", new Info { Title = "DNC Magazine w/ David Pine", Version = "v1" });
                    })
                    .AddSingleton<IOrderService, OrderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"))
                         .AddDebug();

            app.UseMvc()
               .UseSwagger()
               .UseSwaggerUI(config =>
               {
                   config.RoutePrefix = "swagger/ui";
                   config.DocExpansion("none");
                   config.SwaggerEndpoint("/swagger/v1/swagger.json", "DNC Magazine w/ David Pine");
               });
        }
    }
}