using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Predicates
{
    public static class PredicateExtensions
    {
        public static T valueOf<T>(this T source)
        {
            return source;
        }

        public static T moneyString<T>(this T source)
        {
            return source;
        }

        public static bool In<T>(this T source, params T[] values)
        {
            throw new NotImplementedException();
        }

        public static bool NotIn<T>(this T source, params T[] values)
        {
            throw new NotImplementedException();
        }

        public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] values)
        {
            throw new NotImplementedException();
        }

        public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] values)
        {
            throw new NotImplementedException();
        }

        public static bool IsEmpty<T>(this ICollection<T> source)
        {
            throw new NotImplementedException();
        }

        public static bool IsNotEmpty<T>(this List<T> source)
        {
            throw new NotImplementedException();
        }

        public static bool IsDefined<T>(this T source)
        {
            throw new NotImplementedException();
        }

        public static bool IsNotDefined<T>(this T source)
        {
            throw new NotImplementedException();
        }

        public static bool WithinCircle(this GeoJsonGeometry source, params double[] values)
        {
            throw new NotImplementedException();
        }
    }
}
