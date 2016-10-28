using System;
using System.Collections.Generic;

using commercetools.Carts;
using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.TaxCategories
{
    /// <summary>
    /// A SubRate is used to calculate the taxPortions field in a cart or order. It is useful if the total tax of a country is a combination of multiple taxes (e.g. state and local taxes).
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-taxCategories.html#subrate"/>
    public class SubRate
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal? Amount { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public SubRate(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Name = data.name;
            this.Amount = data.amount;
        }

        #endregion
    }
}