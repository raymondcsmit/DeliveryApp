using Core;
using Delivery.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Delivery.Services.Implementation
{
    public class LookupServiceFactory : ILookupServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public LookupServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILookupService<T> Create<T>() where T : class
        {
            var repositoryType = typeof(IRepository<T>);
            var repository = (IRepository<T>)_serviceProvider.GetRequiredService(repositoryType);

            var serviceType = typeof(LookupService<>).MakeGenericType(typeof(T));
            var service = (ILookupService<T>?)Activator.CreateInstance(serviceType, new object[] { repository });
            if (service is null)
            {
                throw new InvalidOperationException($"Failed to create instance of service type {serviceType.FullName}.");
            }

            return service;
        }
    }

}
