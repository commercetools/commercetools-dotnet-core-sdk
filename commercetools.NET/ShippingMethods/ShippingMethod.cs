using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ShippingMethods
{
    /// <summary>
    /// A shipping method defines a specific way of shipping, with different rates for different geographic locations.
    /// </summary>
    /// <remarks>
    /// Example shipping methods are "DHL", "DHL Express" and "UPS".
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-shippingMethods.html#shippingmethod"/>
    public class ShippingMethod
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; private set; }

        [JsonProperty(PropertyName = "taxCategory")]
        public Reference TaxCategory { get; private set; }

        [JsonProperty(PropertyName = "zoneRates")]
        public List<ZoneRate> ZoneRates { get; private set; }

        [JsonProperty(PropertyName = "isDefault")]
        public bool? IsDefault { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ShippingMethod(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Name = data.name;
            this.Description = data.description;
            this.TaxCategory = data.taxCategory != null ? new Reference(data.taxCategory) : null;
            this.ZoneRates = Helper.GetListFromJsonArray<ZoneRate>(data.zoneRates);
            this.IsDefault = data.isDefault;
        }

        #endregion
    }
}
