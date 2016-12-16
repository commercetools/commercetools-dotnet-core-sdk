using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the delete InventoryEntry request. 
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#inventoryentrydeleted-message"/>
    public class InventoryEntryDeletedMessage : Message
    {
        #region Properties

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; private set; }

        [JsonProperty(PropertyName = "supplyChannel")]
        public Reference SupplyChannel { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public InventoryEntryDeletedMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Sku = data.sku;
            this.SupplyChannel = new Reference(data.supplyChannel);
        }

        #endregion
    }
}
