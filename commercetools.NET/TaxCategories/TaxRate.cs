using Newtonsoft.Json;

namespace commercetools.TaxCategories
{
    /// <summary>
    /// TaxRate
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-taxCategories.html#taxrate"/>
    public class TaxRate
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal? Amount { get; private set; }

        [JsonProperty(PropertyName = "includedInPrice")]
        public bool? IncludedInPrice { get; private set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; private set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public TaxRate(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Name = data.name;
            this.Amount = data.amount;
            this.IncludedInPrice = data.includedInPrice;
            this.Country = data.country;
            this.State = data.state;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            TaxRate taxRate = obj as TaxRate;

            if (taxRate == null)
            {
                return false;
            }

            return taxRate.Id.Equals(this.Id);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
