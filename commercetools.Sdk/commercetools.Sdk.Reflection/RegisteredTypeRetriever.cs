using System;
using System.Collections.Generic;
using System.Reflection;

namespace commercetools.Sdk.Util
{
    public class RegisteredTypeRetriever : IRegisteredTypeRetriever
    {
        public IEnumerable<Type> GetRegisteredTypes<T>()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(T));
            IEnumerable<Type> registeredHttpApiCommandTypes = typeof(T).GetAllClassTypesForInterface(assembly);
            return registeredHttpApiCommandTypes;
        }
    }
}