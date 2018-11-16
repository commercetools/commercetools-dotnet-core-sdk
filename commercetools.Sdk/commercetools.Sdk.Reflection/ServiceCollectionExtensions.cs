using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace commercetools.Sdk.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllInterfaceTypes<T>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            Type interfaceType = typeof(T);
            var typesToRegister = interfaceType.GetAllClassTypesForInterface();
            foreach (var type in typesToRegister)
            { 
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }
        }
    }
}
