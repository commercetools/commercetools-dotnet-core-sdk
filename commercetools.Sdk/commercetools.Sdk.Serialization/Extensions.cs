using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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
