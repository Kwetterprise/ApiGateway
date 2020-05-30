namespace Kwetterprise.ApiGateway.Business.Routing
{
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Kwetterprise.ApiGateway.Business.Models;
    using Microsoft.AspNetCore.Http;

    public interface IServiceRouter
    {
        Task<Stream> Route(Service service, string endpoint, HttpContext request);
    }

    public class ExampleRouter : IServiceRouter
    {
        public async Task<Stream> Route(Service service, string endpoint, HttpContext request)
        {
            using var client = new HttpClient();
            return await client.GetStreamAsync("http://google.com");
        }
    }
}
