using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// This action changes the label of a single enum value in a LocalizableEnumType attribute definition. It can update a LocalizableEnumType attribute definition or a Set of LocalizableEnumType attribute definition.
    /// </summary>
    /// <remarks>
    /// All products will be updated to the new label in an eventually consistent way.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#change-the-label-of-a-localizedenumvalue"/>
    public class ChangeLocalizedEnumValueLabelAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// The name of the attribute definition to update.
        /// </summary>
        [JsonProperty(PropertyName = "attributeName")]
        public string AttributeName { get; set; }

        /// <summary>
        /// New Value
        /// </summary>
        /// <remarks>
        /// The new value must be different from the existing value.
        /// </remarks>
        [JsonProperty(PropertyName = "newValue")]
        public LocalizedEnumValue  NewValue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">The name of the attribute definition to update.</param>
        /// <param name="newValue">New Value</param>
        public ChangeLocalizedEnumValueLabelAction(string attributeName, LocalizedEnumValue newValue)
        {
            this.Action = "changeLocalizedEnumValueLabel";
            this.AttributeName = attributeName;
            this.NewValue = newValue;
        }

        #endregion
    }
}
