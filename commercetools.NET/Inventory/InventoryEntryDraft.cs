using commercetools.Common;
using Newtonsoft.Json;
using System;

namespace commercetools.Inventory
{
    /// <summary>
    /// API representation for creating a new inventory entry draft.
    /// </summary>
    /// <see href="https://docs.commercetools.com/http-api-projects-inventory.html#inventoryentrydraft"/>
    public class InventoryEntryDraft
    {
        #region Properties
        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        [JsonProperty(PropertyName = "quantityOnStock")]
        public int QuantityOnStock { get; set; }

        [JsonProperty(PropertyName = "restockableInDays")]
        public int? RestockableInDays { get; set; }

        [JsonProperty(PropertyName = "expectedDelivery")]
        public DateTime? ExpectedDelivery { get; set; }

        [JsonProperty(PropertyName = "supplyChannel")]
        public Reference SupplyChannel { get; set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; set; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sku">SKU</param>
        public InventoryEntryDraft(string sku)
        {
            this.Sku = sku;            
        }
        #endregion
    }
}
