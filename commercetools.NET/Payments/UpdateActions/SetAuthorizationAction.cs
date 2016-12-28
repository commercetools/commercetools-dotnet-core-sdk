using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Sets the amount of money that has been authorized and optionally until when the authorization is valid.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#set-authorization"/>
    public class SetAuthorizationAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// If not provided the amount will be unset.
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; set; }

        /// <summary>
        /// Cannot be set without setting amount, too. If not provided the until date will be unset.
        /// </summary>
        [JsonProperty(PropertyName = "until")]
        public DateTime Until { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetAuthorizationAction()
        {
            this.Action = "setAuthorization";
        }

        #endregion
    }
}
