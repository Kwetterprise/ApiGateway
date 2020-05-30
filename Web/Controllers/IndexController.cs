
namespace Kwetterprise.ApiGateway.Web.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Kwetterprise.ApiGateway.Business.Managing;
    using Kwetterprise.ApiGateway.Business.Models;
    using Kwetterprise.ApiGateway.Business.Routing;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    public class IndexController : Controller
    {
        private readonly ILogger<IndexController> logger;
        private readonly IServiceManager serviceManager;
        private readonly IServiceRouter serviceRouter;

        public IndexController(
            ILogger<IndexController> logger,
            IServiceManager serviceManager,
            IServiceRouter router)
        {
            this.logger = logger;
            this.serviceManager = serviceManager;
            this.serviceRouter = router;
        }

        // [HttpGet("/")]
        // public IActionResult Index()
        // {
        //     return this.View(this.serviceManager.Get("a"));
        // }

        [Route("/api/{service}/{endpoint}")]
        public Task<Stream> Route(string serviceName, string endpoint)
        {
            this.logger.LogInformation($@"Routing: service ""{serviceName}"" and endpoint ""{endpoint}"".");

            var service = this.serviceManager.Get(serviceName);

            if (service is null)
            {
                throw new Exception("Not found.");
            }

            return this.serviceRouter.Route(service, endpoint, this.HttpContext);
        }
    }
}