using System;
using System.Reflection;

namespace SimpleInjector
{
    /// <summary>
    /// This class contains extensions methods for <see cref="Container"/>.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds all concrete implementation types that are assignable from the specified service type to the service collection.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="lifetime">The lifetime.</param>
        public static void RegisterAllTypes<T>(this Container services, Lifestyle lifetime)
        {
            Type type = typeof(T);
            RegisterAllTypes(services, type, lifetime);
        }

        /// <summary>
        /// Adds all concrete implementation types that are assignable from the specified service type to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="type">The service type.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <remarks>In case the specified service type in an open generic type, first the concrete generic type is created based on the retrieved implementation type.</remarks>
        public static void RegisterAllTypes(this Container services, Type type, Lifestyle lifetime)
        {
            Assembly[] assembly = new[] {Assembly.GetAssembly(type)};
            services.RegisterCollection(type, assembly);
        }
    }
}
