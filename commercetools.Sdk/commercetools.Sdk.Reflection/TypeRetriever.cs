using System;
using System.Collections.Generic;
using System.Reflection;

namespace commercetools.Sdk.Registration
{
    internal class TypeRetriever : ITypeRetriever
    {
        public IEnumerable<Type> GetTypes<T>()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(T));
            return typeof(T).GetAllRegisteredTypes(assembly);
        }
    }
}