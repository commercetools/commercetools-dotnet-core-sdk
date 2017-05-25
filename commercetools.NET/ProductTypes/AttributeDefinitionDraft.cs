using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// The representation to be sent to the server when creating a new AttributeDefinition.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#attributedefinitiondraft"/>
    public class AttributeDefinitionDraft
    {
        #region Properties

        /// <summary>
        /// Describes the type of the attribute.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public AttributeType Type { get; set; }

        /// <summary>
        /// The unique name of the attribute used in the API. 
        /// </summary>
        /// <remarks>
        /// The name must be between two and 36 characters long and can contain the ASCII letters A to Z in lowercase or uppercase, digits, underscores (_) and the hyphen-minus (-). It is allowed to have attributes with the same name in two or more ProductTypes. The important constraint is: all fields of the AttributeDefinition need to be the same across all attributes with the same name.
        /// </remarks>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// A human-readable label for the attribute.
        /// </summary>
        [JsonProperty(PropertyName = "label")]
        public LocalizedString Label { get; set; }

        /// <summary>
        /// Whether the attribute is required to have a value.
        /// </summary>
        [JsonProperty(PropertyName = "isRequired")]
        public bool? IsRequired { get; set; }

        /// <summary>
        /// Describes how an attribute or a set of attributes should be validated across all variants of a product.
        /// </summary>
        [JsonProperty(PropertyName = "attributeConstraint")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AttributeConstraint? AttributeConstraint { get; set; }

        /// <summary>
        /// Additional information about the attribute that aids content managers when setting product details.
        /// </summary>
        [JsonProperty(PropertyName = "inputTip")]
        public LocalizedString InputTip { get; set; }

        /// <summary>
        /// Provides a visual representation type for this attribute.
        /// </summary>
        /// <remarks>
        /// Only relevant for text-based attribute types like TextType and LocalizableTextType.
        /// </remarks>
        [JsonProperty(PropertyName = "inputHint")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TextInputHint? InputHint { get; set; }

        /// <summary>
        /// Whether the attribute’s values should generally be enabled in product search.
        /// </summary>
        /// <remarks>
        /// This determines whether the value is stored in products for matching terms in the context of full-text search queries and can be used in facets &amp; filters as part of product search queries. The exact features that are enabled/disabled with this flag depend on the concrete attribute type and are described there.
        /// </remarks>
        [JsonProperty(PropertyName = "isSearchable")]
        public bool? IsSearchable { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Describes the type of the attribute.</param>
        /// <param name="name">The unique name of the attribute used in the API.</param>
        /// <param name="label">A human-readable label for the attribute.</param>
        /// <param name="isRequired">Whether the attribute is required to have a value.</param>
        public AttributeDefinitionDraft(AttributeType type, string name, LocalizedString label, bool isRequired)
        {
            this.Type = type;
            this.Name = name;
            this.Label = label;
            this.IsRequired = isRequired;
        }

        #endregion
    }
}
