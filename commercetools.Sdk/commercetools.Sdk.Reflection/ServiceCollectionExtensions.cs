using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllDerivedTypes<T>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(T));
            services.RegisterAllDerivedTypes<T>(lifetime, assembly);
        }

        public static void RegisterAllDerivedTypes<T>(this IServiceCollection services, ServiceLifetime lifetime, Assembly assembly)
        {
            Type classType = typeof(T);
            var typesToRegister = classType.GetAllDerivedClassTypesForClass(assembly);
            foreach (var type in typesToRegister)
            {
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }
        }

        public static void RegisterAllInterfaceTypes<T>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(T));
            Type interfaceType = typeof(T);
            var typesToRegister = interfaceType.GetAllClassTypesForInterface(assembly);
            foreach (var type in typesToRegister)
            {
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }
        }

        public static void RegisterAllInterfaceTypes(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
        {
            Assembly assembly = Assembly.GetAssembly(interfaceType);
            var typesToRegister = interfaceType.GetAllClassTypesForInterface(assembly);
            foreach (var type in typesToRegister)
            {
                services.Add(new ServiceDescriptor(GetGenericInterface(interfaceType, type), type, lifetime));
            }
        }

        private static Type GetGenericInterface(Type interfaceType, Type classType)
        {
            foreach(Type it in classType.GetInterfaces())
            {
                if (it.IsGenericType)
                {
                    if (it.GetGenericTypeDefinition() == interfaceType)
                    {
                        return it;
                    }
                }
            }
            return null;
        }
    }
}