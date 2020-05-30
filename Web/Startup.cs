namespace Kwetterprise.ApiGateway.Web
{
    using System.IO;
    using Kwetterprise.ApiGateway.Business.Managing;
    using Kwetterprise.ApiGateway.Business.Models;
    using Kwetterprise.ApiGateway.Business.Routing;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using ServiceProvider = Kwetterprise.ApiGateway.Business.Managing.ServiceProvider;

    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddRouting();

            services.AddSingleton(s => new ServiceDiscoveryConfiguration(this.Configuration["ServiceDiscoveryUrl"]));

            services.AddSingleton<IServiceRouter, ExampleRouter>();
            services.AddSingleton<IServiceManager, RollingServiceManager>();
            services.AddSingleton<IServiceProvider, ServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                        "index",
                        "",
                        new { controller = "Index", action = "Indexz", });
                });
        }
    }
}
