using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders
{
    /// <summary>
    /// Deliveries are compilations of information on how the articles are being shipped to the customers.
    /// </summary>
    /// <remarks>
    /// A delivery can contain multiple items. All items in a delivery can be shipped with several parcels. To create a delivery, it is necessary to have a shipment method assigned to the order. A sample use case for a delivery object is to create a delivery note.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#delivery"/>
    public class Delivery
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "items")]
        public List<DeliveryItem> Items { get; private set; }

        [JsonProperty(PropertyName = "parcels")]
        public List<Parcel> Parcels { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Delivery(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.CreatedAt = data.createdAt;
            this.Items = Helper.GetListFromJsonArray<DeliveryItem>(data.items);
            this.Parcels = Helper.GetListFromJsonArray<Parcel>(data.parcels);
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
            Delivery delivery = obj as Delivery;

            if (delivery == null)
            {
                return false;
            }

            return delivery.Id.Equals(this.Id);
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
