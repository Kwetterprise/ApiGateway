using System;
using Xunit;

namespace UnitTest
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Kwetterprise.ApiGateway.Business.Managing;
    using Kwetterprise.ApiGateway.Business.Models;
    using NSubstitute;

    public class RollingServiceManagerTest
    {
        private readonly IServiceProvider serviceProvider;
        private readonly RollingServiceManager serviceManager;

        public RollingServiceManagerTest()
        {
            this.serviceProvider = Substitute.For<IServiceProvider>();
            this.serviceManager = new RollingServiceManager(this.serviceProvider);
        }

        [Fact]
        public async Task Test1()
        {
            var name = "Name";

            var newService = new Service(Guid.NewGuid(), name, "example.com");

            this.serviceProvider.GetServices().Returns(new List<Service> { newService });

            await this.serviceManager.FetchServices();

            var service = this.serviceManager.Get(name);
            Assert.NotNull(service);
            Assert.Equal(newService, service);
        }

        [Fact]
        public void Test2()
        {
            Assert.Null(this.serviceManager.Get("Random Name"));
        }

        [Fact]
        public async Task Service_Is_Correctly_Removed()
        {
            var name = "Name";

            var newService = new Service(Guid.NewGuid(), name, "example.com");

            this.serviceProvider.GetServices().Returns(new List<Service> { newService });

            await this.serviceManager.FetchServices();

            Assert.NotNull(this.serviceManager.Get(name));

            this.serviceProvider.GetServices().Returns(new List<Service>(0));

            await this.serviceManager.FetchServices();

            Assert.Null(this.serviceManager.Get(name));
        }

        [Fact]
        public async Task Double_Register_Service()
        {
            var name = "Name";

            var newService = new Service(Guid.NewGuid(), name, "example.com");

            this.serviceProvider.GetServices().Returns(new List<Service> { newService, newService });

            await this.serviceManager.FetchServices();
        }
    }
}
