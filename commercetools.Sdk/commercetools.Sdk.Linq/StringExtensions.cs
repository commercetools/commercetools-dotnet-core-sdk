using System;
using System.Globalization;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// If string contains a dash (-) or starts with a digit, escape it using backticks (`)
        /// </summary>
        /// <param name="value">name of field or attribute</param>
        /// <returns>string wrapped in backticks if string matched conditions</returns>
        public static string WrapInBackticksIfNeeded(this string value)
        {
            bool wrap = value.Contains("-") || Regex.IsMatch(value, @"^\d");
            var result = wrap ? string.Format(CultureInfo.InvariantCulture, "`{0}`", value) : value;
            return result;
        }

        public static string RemoveQuotes(this string value)
        {
            return value.Replace("\"", string.Empty);
        }

        public static string ToUtcIso8601(this DateTime dt, bool onlyDate = false)
        {
            string dateFormatted;
            if (onlyDate)
            {
                dateFormatted = dt.ToString("yyyy'-'MM'-'dd", CultureInfo.InvariantCulture);
            }
            else
            {
                dateFormatted = dt.ToUniversalTime()
                    .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK", CultureInfo.InvariantCulture);
            }

            return dateFormatted;
        }

        public static string ToIso8601(this TimeSpan time)
        {
            return time.ToString("hh':'mm':'ss'.'fff", CultureInfo.InvariantCulture);
        }
    }
}
