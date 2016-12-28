using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the addInterfaceInteraction update action.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#paymentinteractionadded-message"/>
    public class PaymentInteractionAddedMessage : Message
    {
        #region Properties

        /// <summary>
        /// Interaction
        /// </summary>
        [JsonProperty(PropertyName = "interaction")]
        public CustomFields.CustomFields Interaction { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public PaymentInteractionAddedMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Interaction = new CustomFields.CustomFields(data.interaction);
        }

        #endregion
    }
}
