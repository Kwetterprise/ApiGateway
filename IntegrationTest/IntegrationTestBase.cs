
namespace IntegrationTest
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class IntegrationTestBase : IDisposable
    {
        private IHost? host;

        private HttpClient? client;

        protected HttpClient Client => this.client ?? throw new InvalidOperationException("Host is not started yet.");

        public async Task Start(Action<IServiceCollection>? configureServices = null)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
                    webHost.UseTestServer();

                    // Specify the environment
                    webHost.UseEnvironment("Test");

                    webHost.UseStartup<Kwetterprise.ApiGateway.Web.Startup>();
                    
                    if (configureServices != null)
                    {
                        webHost.ConfigureServices(configureServices);
                    }
                });

            this.host = await hostBuilder.StartAsync();

            this.client = this.host.GetTestClient();
        }


        public void Dispose()
        {
            this.host?.Dispose();
        }
    }
}
