using System;
using System.Globalization;
using System.Linq;
using FluentValidation.Validators;

namespace commercetools.Sdk.Validation
{
    public class CountryValidator : PropertyValidator
    {
        private readonly RegionInfo[] regionInfos;

        public CountryValidator(): base("{PropertyName} has invalid country")
        {
            this.regionInfos = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(x => !x.Equals(CultureInfo.InvariantCulture))
                .Where(x => !x.IsNeutralCulture).Select(x => new RegionInfo(x.Name)).ToArray();
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var country = context.PropertyValue as string;
            if (string.IsNullOrEmpty(country))
            {
                return true;
            }

            return this.regionInfos.Any(x => x.Name.Equals(country, StringComparison.InvariantCulture));
        }
    }
}
