using System;
using System.Globalization;
using System.Linq;

namespace commercetools.Sdk.Serialization
{
    internal static class Extensions
    {
        // TODO Move this to another project
        public static bool IsValidLanguageTag(this string name)
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            if (cultures.Select(c => c.TwoLetterISOLanguageName).Contains(name))
            {
                return true;
            }
            return false;
        }
    }
}