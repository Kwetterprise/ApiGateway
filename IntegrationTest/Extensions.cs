using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest
{
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static IServiceCollection Replace<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);

            services.Add(descriptorToAdd);

            return services;
        }

        public static IServiceCollection Replace<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> factory,
            ServiceLifetime lifetime)
            where TService : class
            where TImplementation : TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var objectFactory = new Func<IServiceProvider, object>(p => factory(p));

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), objectFactory, lifetime);

            services.Add(descriptorToAdd);

            return services;
        }
    }
}
