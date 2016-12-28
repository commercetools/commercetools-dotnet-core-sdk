using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// Money
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-types.html#money"/>
    public class Money
    {
        #region Properties

        [JsonProperty(PropertyName = "currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty(PropertyName = "centAmount")]
        public int? CentAmount { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public Money()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Money(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.CurrencyCode = data.currencyCode;
            this.CentAmount = data.centAmount;
        }

        #endregion
    }
}
