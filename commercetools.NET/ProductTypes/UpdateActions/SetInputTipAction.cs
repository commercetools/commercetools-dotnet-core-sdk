using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// Allows to set additional information about the specified attribute that aids content managers when setting product details.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#set-attributedefinition-inputtip"/>
    public class SetInputTipAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// The name of the attribute definition to update.
        /// </summary>
        [JsonProperty(PropertyName = "attributeName")]
        public string AttributeName { get; set; }

        /// <summary>
        /// Input Tip
        /// </summary>
        [JsonProperty(PropertyName = "inputTip")]
        public LocalizedString InputTip { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">The name of the attribute definition to update.</param>
        public SetInputTipAction(string attributeName)
        {
            this.Action = "setInputTip";
            this.AttributeName = attributeName;
        }

        #endregion
    }
}
