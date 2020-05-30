namespace Kwetterprise.ApiGateway.Business.Managing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Kwetterprise.ApiGateway.Business.Models;
    using Microsoft.Extensions.Logging;

    public interface IServiceProvider
    {
        Task<List<Service>> GetServices();
    }

    public class ServiceProvider : IServiceProvider
    {
        private readonly HttpClient client = new HttpClient();

        private readonly ILogger<ServiceProvider> logger;
        private readonly ServiceDiscoveryConfiguration serviceDiscoveryConfiguration;

        public ServiceProvider(ILogger<ServiceProvider> logger, ServiceDiscoveryConfiguration serviceDiscoveryConfiguration)
        {
            this.logger = logger;
            this.serviceDiscoveryConfiguration = serviceDiscoveryConfiguration;
        }

        public async Task<List<Service>> GetServices()
        {
            string response;
            try
            {
                response = await this.client.GetStringAsync(this.serviceDiscoveryConfiguration.GetAllUrl);
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevel.Error, e, "Error!!!");
                Debugger.Break();
                // TODO
                throw;
            }

            return JsonSerializer.Deserialize<List<Service>>(response);
        }
    }

    public class ServiceDiscoveryConfiguration
    {
        public ServiceDiscoveryConfiguration(string baseUrl)
        {
            this.GetAllUrl = baseUrl + "/getAll";
        }

        public string GetAllUrl { get; }
    }

    public class RollingServiceManager : IServiceManager
    {
        private readonly IServiceProvider serviceProvider;

        private Dictionary<Guid, Service> services = new Dictionary<Guid, Service>();
        private Dictionary<string, List<Guid>> nameToGuid = new Dictionary<string, List<Guid>>();
        private Dictionary<string, int> cycle = new Dictionary<string, int>();

        public RollingServiceManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

        }

        public async Task FetchServices()
        {
            var newServices = await this.serviceProvider.GetServices();

            newServices = newServices.Distinct().ToList();

            this.services = newServices.ToDictionary(x => x.Guid);
            this.nameToGuid = newServices.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.Select(y => y.Guid).ToList());
            this.cycle = newServices.Select(x => x.Name).Distinct().ToDictionary(x => x, x => 0);
        }

        public Service? Get(string name)
        {
            if (!this.nameToGuid.ContainsKey(name))
            {
                return null;
            }

            var guids = this.nameToGuid[name];

            var currCycle = this.cycle[name];

            var service = this.services[guids[currCycle]];

            this.cycle[name]++;

            if (currCycle + 1 == guids.Count)
            {
                this.cycle[name] = 0;
            }

            return service;
        }
    }
}