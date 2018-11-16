using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// TODO Not happy with project name; I did not want to name it Util either
namespace commercetools.Sdk.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetAllClassTypesForInterface(this Type interfaceType)
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
    }
}
