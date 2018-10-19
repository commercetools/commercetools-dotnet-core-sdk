using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class Filter
    {
    }

    // TODO Move this to another project so that it is not imported everywhere
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

        public static bool Range<T>(this T source, T? from, T? to) where T: struct
        {
            throw new NotImplementedException();
        }

        public static bool isOnStockInChannels<T>(this T source, params string[] values)
        {
            throw new NotImplementedException();
        }
    }
}
