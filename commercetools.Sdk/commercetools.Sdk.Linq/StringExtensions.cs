using System;
using System.Globalization;
using System.Xml;

namespace commercetools.Sdk.Linq
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > 1)
            {
                return char.ToLowerInvariant(value[0]) + value.Substring(1);
            }

            return value;
        }

        public static string WrapInQuotes(this string value)
        {
            return string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value);
        }

        public static string RemoveQuotes(this string value)
        {
            return value.Replace("\"", string.Empty);
        }

        public static string ToUtcIso8601(this DateTime dt)
        {
            return dt.ToUniversalTime()
                .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK", CultureInfo.InvariantCulture);
        }
    }
}
