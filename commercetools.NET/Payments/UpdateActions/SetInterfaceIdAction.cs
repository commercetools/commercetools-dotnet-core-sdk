using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Sets the identifier that is used by the interface that manages the payment (usually the PSP).
    /// </summary>
    /// <remarks>
    /// Cannot be changed once it has been set. The combination of interfaceId and PaymentMethodInfo paymentInterface must be unique.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#set-interfaceid"/>
    public class SetInterfaceIdAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Interface ID
        /// </summary>
        [JsonProperty(PropertyName = "interfaceId")]
        public string InterfaceId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="interfaceId">Interface ID</param>
        public SetInterfaceIdAction(string interfaceId)
        {
            this.Action = "setInterfaceId";
            this.InterfaceId = interfaceId;
        }

        #endregion
    }
}
