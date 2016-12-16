using System.Collections.Generic;

using commercetools.TaxCategories;

using Newtonsoft.Json;

namespace commercetools.Carts
{
    /// <summary>
    /// ExternalTaxRateDraft
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#externaltaxratedraft"/>
    public class ExternalTaxRateDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal? Amount { get; set; }

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
        public ExternalTaxRateDraft(string name, string country)
        {
            this.Name = name;
            this.Country = country;
        }

        #endregion
    }
}
