namespace commercetools.Sdk.Domain.Validation
{
    using System;
    using System.Globalization;
    using System.Linq;

    public class CountryValidator : ICountryValidator
    {
        private readonly RegionInfo[] regionInfos;

        public CountryValidator()
        {
            this.regionInfos = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(x => !x.Equals(CultureInfo.InvariantCulture))
                .Where(x => !x.IsNeutralCulture).Select(x => new RegionInfo(x.Name)).ToArray();
        }

        public bool IsCountryValid(string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return true;
            }

            return this.regionInfos.Any(x => x.Name.Equals(country, StringComparison.InvariantCulture));
        }
    }
}
