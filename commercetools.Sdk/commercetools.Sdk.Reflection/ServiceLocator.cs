using System;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Registration
{
    /// <summary>
    /// This class implements the service locator pattern.
    /// </summary>
    /// <remarks>
    /// Since some objects are created directly by the implementors of the SDK, dependencies cannot be automatically injected via constructor.
    /// Therefore, the service locator pattern is used to retrieve the needed implementations.
    /// </remarks>
    public class ServiceLocator
    {
        private static ServiceProvider serviceProvider;
        private ServiceProvider currentServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="currentServiceProvider">The current service provider.</param>
        public ServiceLocator(ServiceProvider currentServiceProvider)
        {
            this.currentServiceProvider = currentServiceProvider;
        }

        /// <summary>
        /// Gets the <see cref="ServiceLocator"/> instance for the static service provider.
        /// </summary>
        /// <value>
        /// The <see cref="ServiceLocator"/> instance.
        /// </value>
        public static ServiceLocator Current
        {
            get
            {
                return new ServiceLocator(serviceProvider);
            }
        }

        /// <summary>
        /// Sets the static service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public static void SetServiceLocatorProvider(ServiceProvider serviceProvider)
        {
            ServiceLocator.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the service object of the specified service type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>The service object of the specified service type.</returns>
        public object GetService(Type serviceType)
        {
            return this.currentServiceProvider.GetService(serviceType);
        }

        /// <summary>
        /// Gets the service object of the specified service type.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <returns>The service object of the specified service type.</returns>
        public TService GetService<TService>()
        {
            return this.currentServiceProvider.GetService<TService>();
        }
    }
}
