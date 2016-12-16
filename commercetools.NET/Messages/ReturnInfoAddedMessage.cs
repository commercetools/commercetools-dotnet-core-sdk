using commercetools.Orders;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the addParcelToDelivery update action.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#returninfoadded-message"/>
    public class ReturnInfoAddedMessage : Message
    {
        #region Properties

        /// <summary>
        /// ReturnInfo
        /// </summary>
        [JsonProperty(PropertyName = "returnInfo")]
        public ReturnInfo ReturnInfo { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ReturnInfoAddedMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.ReturnInfo = new ReturnInfo(data.returnInfo);
        }

        #endregion
    }
}
