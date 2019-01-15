namespace commercetools.Sdk.Domain.Validation
{
    using System.Globalization;
    using System.Linq;

    public class CultureValidator : ICultureValidator
    {
        private readonly CultureInfo[] cultures;

        public CultureValidator()
        {
            this.cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        }

        public bool IsCultureValid(string culture)
        {
            if (string.IsNullOrEmpty(culture))
            {
                return true;
            }

            // Both two letter language name and full culture name can be accepted.
            // For example, zh-Hant and zh are both valid.
            return this.cultures.Select(c => c.TwoLetterISOLanguageName).Contains(culture) || 
                this.cultures.Select(c => c.Name).Contains(culture);
        }
    }
}
