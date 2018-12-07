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
                .Where(x => !x.IsNeutralCulture).Select(x => new RegionInfo(x.LCID)).ToArray();
        }

        public bool IsCountryValid(string country)
        {
            return this.regionInfos.Any(x => x.Name.Equals(country, StringComparison.InvariantCulture));
        }
    }
}
