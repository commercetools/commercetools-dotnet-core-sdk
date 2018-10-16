using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class Filter
    {
    }

    public static class FilterExtensions
    {
        public static bool Missing<T>(this IEnumerable<T> source)
        {
            throw new NotImplementedException();
        }

        public static bool Exists<T>(this IEnumerable<T> source)
        {
            throw new NotImplementedException();
        }

        public static bool Subtree<T>(this T source, string value)
        {
            throw new NotImplementedException();
        }

        public static bool Range(this int source, int? from, int? to)
        {
            throw new NotImplementedException();
        }

        public static bool Range(this double source, double? from, double? to)
        {
            throw new NotImplementedException();
        }
    }
}
