using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// TODO Not happy with project name; I did not want to name it Util either
namespace commercetools.Sdk.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetAllClassTypesForInterface(this Type interfaceType, Assembly assembly)
        {
            List<Type> classTypes = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass && type.GetInterfaces().Contains(interfaceType))
                {
                    classTypes.Add(type);
                }
            }
            return classTypes;
        }

        public static IEnumerable<Type> GetAllDerivedClassTypesForClass(this Type classType, Assembly assembly)
        {
            List<Type> classTypes = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (type != classType && classType.IsAssignableFrom(type))
                {
                    classTypes.Add(type);
                }
            }
            return classTypes;
        }
    }
}
