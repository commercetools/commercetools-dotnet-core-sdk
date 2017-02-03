using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// Adds an enum to the values of EnumType. It can update an EnumType attribute definition or a Set of EnumType attribute definition.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#add-plainenumvalue-to-attributedefinition"/>
    public class AddPlainEnumValueAction : UpdateAction
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
        public PlainEnumValue Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">The name of the attribute definition to update.</param>
        /// <param name="value">Value</param>
        public AddPlainEnumValueAction(string attributeName, PlainEnumValue value)
        {
            this.Action = "addPlainEnumValue";
            this.AttributeName = attributeName;
            this.Value = value;
        }

        #endregion
    }
}
