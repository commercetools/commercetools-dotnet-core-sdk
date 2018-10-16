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
        public static bool Missing<T>(this List<T> source)
        {
            throw new NotImplementedException();
        }

        public static bool Exists<T>(this List<T> source)
        {
            throw new NotImplementedException();
        }

        public static bool Subtree<T>(this T source, string value)
        {
            throw new NotImplementedException();
        }

        public static bool Range<T>(this T source, T from, T to)
        {
            throw new NotImplementedException();
        }
    }
}
