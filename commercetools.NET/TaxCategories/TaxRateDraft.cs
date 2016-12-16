using System.Collections.Generic;

using Newtonsoft.Json;

namespace commercetools.TaxCategories
{
    /// <summary>
    /// API representation for creating a new tax rate.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-taxCategories.html#taxratedraft"/>
    public class TaxRateDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal? Amount { get; set; }

        [JsonProperty(PropertyName = "includedInPrice")]
        public bool IncludedInPrice { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "subRates")]
        public List<SubRate> SubRates { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="includedInPrice">Included in price</param>
        /// <param name="country">A two-digit country code as per ISO 3166-1 alpha-2.</param>
        public TaxRateDraft(string name, bool includedInPrice, string country)
        {
            this.Name = name;
            this.IncludedInPrice = includedInPrice;
            this.Country = country;
        }

        #endregion
    }
}
