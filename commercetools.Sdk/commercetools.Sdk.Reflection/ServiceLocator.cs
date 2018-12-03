using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Util
{
    // Since some objects are created directly by the implementors of the SDK, dependencies cannot be automatically injected via constructor.
    // Therefore, the service locator pattern is used to retrieve the needed implementations.
    public class ServiceLocator
    {
        private ServiceProvider currentServiceProvider;
        private static ServiceProvider serviceProvider;

        public ServiceLocator(ServiceProvider currentServiceProvider)
        {
            this.currentServiceProvider = currentServiceProvider;
        }

        public static ServiceLocator Current
        {
            get
            {
                return new ServiceLocator(serviceProvider);
            }
        }

        public static void SetLocatorProvider(ServiceProvider serviceProvider)
        {
            ServiceLocator.serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return currentServiceProvider.GetService(serviceType);
        }

        public TService GetService<TService>()
        {
            return currentServiceProvider.GetService<TService>();
        }
    }
}
