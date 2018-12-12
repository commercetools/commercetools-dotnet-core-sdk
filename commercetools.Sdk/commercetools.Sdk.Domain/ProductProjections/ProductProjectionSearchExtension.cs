using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.ProductProjections
{
    public static class ProductProjectionSearchExtension
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

        public static bool Range<T>(this T source, T? from, T? to) where T : struct
        {
            throw new NotImplementedException();
        }

        public static bool IsOnStockInChannels<T>(this T source, params string[] values)
        {
            throw new NotImplementedException();
        }
    }
}
