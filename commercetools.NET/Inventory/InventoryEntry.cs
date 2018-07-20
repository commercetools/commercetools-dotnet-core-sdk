using commercetools.Common;
using commercetools.CustomFields;
using Newtonsoft.Json;
using System;

namespace commercetools.Inventory
{
    /// <summary>
    /// Inventory allows to track stock quantity per SKU and optionally per supply channel.
    /// </summary>
    /// <see href="https://docs.commercetools.com/http-api-projects-inventory.html#inventoryentry"/>
    public class InventoryEntry
    {
        #region Properties
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; private set; }

        [JsonProperty(PropertyName = "supplyChannel")]
        public Reference SupplyChannel { get; private set; }

        [JsonProperty(PropertyName = "quantityOnStock")]
        public int QuantityOnStock { get; private set; }

        [JsonProperty(PropertyName = "availableQuantity")]
        public int AvailableQuantity { get; private set; }

        [JsonProperty(PropertyName = "restockableInDays")]
        public int? RestockableInDays { get; private set; }

        [JsonProperty(PropertyName = "expectedDelivery")]
        public DateTime? ExpectedDelivery { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public InventoryEntry(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Sku = data.sku;
            this.SupplyChannel = data.supplyChannel != null ? new Reference(data.supplyChannel) : null;
            this.QuantityOnStock = data.quantityOnStock;
            this.AvailableQuantity = data.availableQuantity;
            this.RestockableInDays = data.restockableInDays;
            this.ExpectedDelivery = data.expectedDelivery;
            this.Custom = new CustomFields.CustomFields(data.custom);
        }
        #endregion
    }
}
