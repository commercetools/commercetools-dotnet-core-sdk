using commercetools.Sdk.Extensions;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Test.Helpers
{
    public static class ReflectionHelper
    {
        public static IEnumerable<T> GetInstancesForInterface<T>()
        {
            Type interfaceType = typeof(T);
            IEnumerable<Type> types = interfaceType.GetAllClassTypesForInterface();
            List<T> instances = new List<T>();
            foreach (Type type in types)
            {
                object instance = Activator.CreateInstance(type);
                instances.Add((T)instance);
            }
            return instances;
        }
    }
}
