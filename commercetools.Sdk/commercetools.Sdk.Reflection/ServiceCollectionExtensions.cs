using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllTypes<T>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            Type type = typeof(T);
            RegisterAllTypes(services, type, lifetime);
        }

        public static void RegisterAllTypes(this IServiceCollection services, Type type, ServiceLifetime lifetime)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            var typesToRegister = type.GetAllRegisteredTypes(assembly);
            foreach (var implementationType in typesToRegister)
            {
                Type typeToRegister = type;
                if (type.IsGenericTypeDefinition)
                {
                    typeToRegister = GetTypeForGenericTypeDefinition(type, implementationType);
                }

                services.Add(new ServiceDescriptor(typeToRegister, implementationType, lifetime));
            }
        }

        // get IDecoratorTypeRetriever<Attribute> for IDecoratorTypeRetriever<>
        public static Type GetTypeForGenericTypeDefinition(Type genericTypeDefinition, Type implementationType)
        {
            // TODO Implement for classes as well
            foreach (Type type in implementationType.GetInterfaces())
            {
                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == genericTypeDefinition)
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }
}