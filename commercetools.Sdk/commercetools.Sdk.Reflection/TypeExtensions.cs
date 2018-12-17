using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetAllClassTypesForInterface(this Type interfaceType, Assembly assembly)
        {
            List<Type> classTypes = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (interfaceType.IsGenericType)
                {
                    if (type.IsClass && !type.IsAbstract && type.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition()).Contains(interfaceType))
                    {
                        classTypes.Add(type);
                    }
                }
                else
                {
                    if (type.IsClass && !type.IsAbstract && type.GetInterfaces().Contains(interfaceType))
                    {
                        classTypes.Add(type);
                    }
                }
            }
            return classTypes;
        }

        public static IEnumerable<Type> GetAllRegisteredTypes(this Type type, Assembly assembly)
        {
            if (type.IsInterface)
            {
                return type.GetAllClassTypesForInterface(assembly);
            }
            if (type.IsClass)
            {
                return type.GetAllDerivedClassTypesForClass(assembly);
            }
            return new List<Type>();
        }

        public static IEnumerable<Type> GetAllDerivedClassTypesForClass(this Type classType, Assembly assembly)
        {
            List<Type> classTypes = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (type != classType && !type.IsAbstract && classType.IsAssignableFrom(type))
                {
                    classTypes.Add(type);
                }
            }
            return classTypes;
        }
    }
}