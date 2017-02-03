using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// This action changes the label of a single enum value in an EnumType attribute definition. It can update an EnumType attribute definition or a Set of EnumType attribute definition.
    /// </summary>
    /// <remarks>
    /// All products will be updated to the new label in an eventually consistent way.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#change-the-label-of-an-enumvalue"/>
    public class ChangePlainEnumValueLabelAction : UpdateAction
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
        public PlainEnumValue NewValue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">The name of the attribute definition to update.</param>
        /// <param name="newValue">New Value</param>
        public ChangePlainEnumValueLabelAction(string attributeName, PlainEnumValue newValue)
        {
            this.Action = "changePlainEnumValueLabel";
            this.AttributeName = attributeName;
            this.NewValue = newValue;
        }

        #endregion
    }
}
