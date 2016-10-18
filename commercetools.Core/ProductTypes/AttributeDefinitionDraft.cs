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

        [JsonProperty(PropertyName = "type")]
        public AttributeType Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public LocalizedString Label { get; set; }

        [JsonProperty(PropertyName = "isRequired")]
        public bool? IsRequired { get; set; }

        [JsonProperty(PropertyName = "attributeConstraint")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AttributeConstraint? AttributeConstraint { get; set; }

        [JsonProperty(PropertyName = "inputHint")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TextInputHint InputHint { get; set; }

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
