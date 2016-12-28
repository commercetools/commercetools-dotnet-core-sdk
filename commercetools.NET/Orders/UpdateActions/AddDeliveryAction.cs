using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders.UpdateActions
{
    /// <summary>
    /// Add delivery
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#add-delivery"/>
    public class AddDeliveryAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Items to add
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public List<DeliveryItem> Items { get; set; }

        /// <summary>
        /// List of parcels
        /// </summary>
        [JsonProperty(PropertyName = "parcels")]
        public List<Parcel> Parcels { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">Items to add</param>
        public AddDeliveryAction(List<DeliveryItem> items)
        {
            this.Action = "addDelivery";
            this.Items = items;
        }

        #endregion
    }
}
