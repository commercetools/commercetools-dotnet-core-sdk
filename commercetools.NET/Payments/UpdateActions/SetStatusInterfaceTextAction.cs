using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Sets a text, given by the PSP, that describes the current status.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-payments.html#set-statusinterfacetext"/>
    public class SetStatusInterfaceTextAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Interface Text
        /// </summary>
        [JsonProperty(PropertyName = "interfaceText ")]
        public string InterfaceText { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="interfaceText">Interface Text</param>
        public SetStatusInterfaceTextAction(string interfaceText)
        {
            this.Action = "setStatusInterfaceText";
            this.InterfaceText = interfaceText;
        }

        #endregion
    }
}
