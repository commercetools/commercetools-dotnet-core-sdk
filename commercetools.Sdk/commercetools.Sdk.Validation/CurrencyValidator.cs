using System;
using System.Globalization;
using System.Linq;
using FluentValidation.Validators;

namespace commercetools.Sdk.Validation
{
    public class CurrencyValidator : PropertyValidator
    {
        private readonly RegionInfo[] regionInfos;

        public CurrencyValidator(): base("{PropertyName} isn't a valid currency")
        {
            this.regionInfos = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(x => !x.Equals(CultureInfo.InvariantCulture))
                .Where(x => !x.IsNeutralCulture).Select(x => new RegionInfo(x.Name)).ToArray();
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var currency = context.PropertyValue as string;
            if (string.IsNullOrEmpty(currency))
            {
                return true;
            }

            return this.regionInfos.Any(x => x.ISOCurrencySymbol.Equals(currency, StringComparison.InvariantCulture));
        }
    }
}
