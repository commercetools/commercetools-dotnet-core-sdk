using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace commercetools.Sdk.Reflection
{
    public class TypeRetriever
    {
        private static IEnumerable<Type> GetAllClassTypesForInterface(Type interfaceType)
        {
            List<Type> classTypes = new List<Type>();
            IEnumerable<Type> types =
            from a in AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            select t;
            foreach (Type type in types)
            {
                if (type.IsClass && type.GetInterfaces().Contains(interfaceType))
                {
                    classTypes.Add(type);
                }
            }
            return classTypes;
        }

        public static IEnumerable<T> GetInstancesForInterface<T>()
        {
            Type interfaceType = typeof(T);
            IEnumerable<Type> types = GetAllClassTypesForInterface(interfaceType);
            List<T> instances = new List<T>();
            foreach(Type type in types)
            {
                object instance = Activator.CreateInstance(type);
                instances.Add((T)instance);                
            }
            return instances;
        }
    }
}
