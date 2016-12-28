using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts
{
    /// <summary>
    /// Represents the portions that sum up to the totalGross field of a TaxedPrice.
    /// </summary>
    /// <remarks>
    /// The portions are calculated from the TaxRates. If a tax rate has SubRates, they are used and can be identified by name. Tax portions from line items that have the same rate and name will be accumulated to the same tax portion.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#taxportion"/>
    public class TaxPortion
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "rate")]
        public decimal? Rate { get; private set; }

        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public TaxPortion(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Name = data.name;
            this.Rate = data.rate;
            this.Amount = new Money(data.amount);
        }

        #endregion
    }
}
