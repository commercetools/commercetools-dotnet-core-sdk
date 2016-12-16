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
        public string Name { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal? Amount { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SubRate()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public SubRate(dynamic data)
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
