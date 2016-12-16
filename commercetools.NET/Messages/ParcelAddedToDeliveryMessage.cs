using commercetools.Orders;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the addParcelToDelivery update action.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#parceladdedtodelivery-message"/>
    public class ParcelAddedToDeliveryMessage : Message
    {
        #region Properties

        /// <summary>
        /// Delivery
        /// </summary>
        [JsonProperty(PropertyName = "delivery")]
        public Delivery Delivery { get; private set; }

        /// <summary>
        /// Parcel
        /// </summary>
        [JsonProperty(PropertyName = "parcel")]
        public Parcel Parcel { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ParcelAddedToDeliveryMessage(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Delivery = new Delivery(data.delivery);
            this.Parcel = new Parcel(data.parcel);
        }

        #endregion
    }
}
