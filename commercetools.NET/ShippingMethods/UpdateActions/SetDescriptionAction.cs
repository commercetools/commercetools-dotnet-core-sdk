using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ShippingMethods.UpdateActions
{
    /// <summary>
    /// Set Description
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-shippingMethods.html#set-description"/>
    public class SetDescriptionAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetDescriptionAction()
        {
            this.Action = "setDescription";
        }

        #endregion
    }
}
