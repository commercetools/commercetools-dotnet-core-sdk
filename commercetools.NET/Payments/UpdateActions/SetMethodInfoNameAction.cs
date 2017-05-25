using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Sets a human-readable, localizable name for the payment method, e.g. 'Credit Card'.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-payments.html#set-methodinfoname"/>
    public class SetMethodInfoNameAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        /// <remarks>
        /// If not provided, the name is unset.
        /// </remarks>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetMethodInfoNameAction()
        {
            this.Action = "setMethodInfoName";
        }

        #endregion
    }
}
