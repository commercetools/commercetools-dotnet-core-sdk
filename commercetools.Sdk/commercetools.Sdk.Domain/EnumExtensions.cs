using System.ComponentModel;
using System.Reflection;
using commercetools.Sdk.Domain;

namespace System
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            return value.ToString();
        }

        public static object GetEnum(this string value, Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (GetDescription((Enum)Enum.Parse(enumType, name)).Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }
            }

            throw new ArgumentException();
        }
        
        public static T GetEnum<T>(this string value) where T : Enum
        {
            string[] names = Enum.GetNames(typeof(T));
            foreach (string name in names)
            {
                if (GetDescription((Enum)Enum.Parse(typeof(T), name)).Equals(value))
                {
                    return (T)Enum.Parse(typeof(T), name);
                }
            }

            throw new ArgumentException();
        }
    }
}
