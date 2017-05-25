using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Sets the code, given by the PSP, that describes the current status.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-payments.html#set-statusinterfacecode"/>
    public class SetStatusInterfaceCodeAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Interface Code
        /// </summary>
        [JsonProperty(PropertyName = "interfaceCode")]
        public string InterfaceCode { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="interfaceCode">Interface Code</param>
        public SetStatusInterfaceCodeAction(string interfaceCode)
        {
            this.Action = "setStatusInterfaceCode";
            this.InterfaceCode = interfaceCode;
        }

        #endregion
    }
}
