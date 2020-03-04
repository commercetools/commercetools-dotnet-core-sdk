using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace commercetools.Sdk.Linq
{
    internal static class EnumExtensions
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

        public static bool IsListOrArray(this Type t)
        {
            return t.IsArray || (t != typeof(string) && typeof(IEnumerable).IsAssignableFrom(t));
        }
    }
}