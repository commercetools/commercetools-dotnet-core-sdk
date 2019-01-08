using System;
using System.Collections.Generic;
using System.Reflection;

namespace commercetools.Sdk.Registration
{
    /// <summary>
    /// Retrieves the types from an assembly.
    /// </summary>
    internal class TypeRetriever : ITypeRetriever
    {
        /// <summary>
        /// Gets the types from the same assembly as T that T is assignable from.
        /// </summary>
        /// <typeparam name="T">A type.</typeparam>
        /// <returns>The list of types.</returns>
        public IEnumerable<Type> GetTypes<T>()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(T));
            return typeof(T).GetAllConcreteAssignableTypes(assembly);
        }
    }
}