using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the transitionState update action. 
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#productstatetransition-message"/>
    public class ProductStateTransitionMessage : Message
    {
        #region Properties

        /// <summary>
        /// Reference
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public Reference Reference { get; private set; }

        /// <summary>
        /// Force
        /// </summary>
        [JsonProperty(PropertyName = "force")]
        public bool? Force { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductStateTransitionMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Reference = new Reference(data.state);
            this.Force = data.force;
        }

        #endregion
    }
}
