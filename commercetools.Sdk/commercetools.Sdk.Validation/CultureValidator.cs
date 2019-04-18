using System.Globalization;
using System.Linq;
using FluentValidation.Validators;

namespace commercetools.Sdk.Validation
{
    public class CultureValidator : PropertyValidator
    {
        private readonly CultureInfo[] cultures;

        public CultureValidator(): base("{PropertyName} isn't a valid locale")
        {
            this.cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var culture = context.PropertyValue as string;
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
