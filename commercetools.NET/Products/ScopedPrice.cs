using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// Scoped price is returned as a part of a variant in product search (when price selector is used).
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#scopedprice"/>
    public class ScopedPrice
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "value")]
        public Money Value { get; private set; }

        [JsonProperty(PropertyName = "currentValue")]
        public Money CurrentValue { get; private set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; private set; }

        [JsonProperty(PropertyName = "customerGroup")]
        public Reference CustomerGroup { get; private set; }

        [JsonProperty(PropertyName = "channel")]
        public Reference Channel { get; private set; }

        [JsonProperty(PropertyName = "validFrom")]
        public DateTime? ValidFrom { get; private set; }

        [JsonProperty(PropertyName = "validUntil")]
        public DateTime? ValidUntil { get; private set; }

        [JsonProperty(PropertyName = "discounted")]
        public DiscountedPrice Discounted { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ScopedPrice(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Value = new Money(data.value);
            this.CurrentValue = new Money(data.currentValue);
            this.Country = data.country;
            this.CustomerGroup = new Reference(data.customerGroup);
            this.Channel = new Reference(data.channel);
            this.ValidFrom = data.validFrom;
            this.ValidUntil = data.validUntil;
            this.Discounted = new DiscountedPrice(data.discounted);
            this.Custom = new CustomFields.CustomFields(data.custom);
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
            ScopedPrice scopedPrice = obj as ScopedPrice;

            if (scopedPrice == null)
            {
                return false;
            }

            return scopedPrice.Id.Equals(this.Id);
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
