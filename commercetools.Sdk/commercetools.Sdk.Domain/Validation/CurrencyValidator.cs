namespace commercetools.Sdk.Domain.Validation
{
    using System;
    using System.Globalization;
    using System.Linq;

    public class CurrencyValidator : ICurrencyValidator
    {
        private readonly RegionInfo[] regionInfos;

        public CurrencyValidator()
        {
            this.regionInfos = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(x => !x.Equals(CultureInfo.InvariantCulture))
                .Where(x => !x.IsNeutralCulture).Select(x => new RegionInfo(x.LCID)).ToArray();
        }

        public bool IsCurrencyValid(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                return true;
            }

            return this.regionInfos.Any(x => x.ISOCurrencySymbol.Equals(currency, StringComparison.InvariantCulture));
        }
    }
}
