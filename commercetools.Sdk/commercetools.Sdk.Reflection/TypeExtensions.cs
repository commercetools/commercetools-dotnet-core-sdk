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

        // TODO Remove, seems not to be needed
        public static IEnumerable<Type> GetParentTypes(this Type type)
        {
            // is there any base type?
            if (type == null)
            {
                yield break;
            }

            // return all implemented or inherited interfaces
            foreach (var i in type.GetInterfaces())
            {
                yield return i;
                var currentInterfaceBaseType = i.BaseType;
                while (currentInterfaceBaseType != null)
                {
                    yield return currentInterfaceBaseType;
                    currentInterfaceBaseType = currentInterfaceBaseType.BaseType;
                }
            }

            // return all inherited types
            var currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }
    }
}
