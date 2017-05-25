using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// Adds an localizable enum to the values of LocalizableEnumType. It can update a LocalizableEnum attribute definition or a Set of LocalizableEnumType attribute definition.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#add-localizableenumvalue-to-attributedefinition"/>
    public class AddLocalizedEnumValueAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// The name of the attribute definition to update.
        /// </summary>
        [JsonProperty(PropertyName = "attributeName")]
        public string AttributeName { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public LocalizedEnumValue Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">The name of the attribute definition to update.</param>
        /// <param name="value">Value</param>
        public AddLocalizedEnumValueAction(string attributeName, LocalizedEnumValue value)
        {
            this.Action = "addLocalizedEnumValue";
            this.AttributeName = attributeName;
            this.Value = value;
        }

        #endregion
    }
}
