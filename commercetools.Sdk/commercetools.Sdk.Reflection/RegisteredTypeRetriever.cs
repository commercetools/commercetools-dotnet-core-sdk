using System;
using System.Collections.Generic;
using System.Reflection;

namespace commercetools.Sdk.Extensions
{
    public class RegisteredTypeRetriever : IRegisteredTypeRetriever
    {
        private readonly Assembly assembly;

        public RegisteredTypeRetriever(Assembly assembly)
        {
            this.assembly = assembly;    
        }

        public IEnumerable<Type> GetRegisteredTypes<T>()
        {
            IEnumerable<Type> registeredHttpApiCommandTypes = typeof(T).GetAllClassTypesForInterface(assembly);
            return registeredHttpApiCommandTypes;
        }
    }
}