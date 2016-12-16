using System;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// Attribute definitions describe a product attribute and allow you to define some meta-information associated with the attribute (like whether it should be searchable or its constraints).
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#attributedefinition"/>
    public class AttributeDefinition
    {
        #region Properties

        [JsonProperty(PropertyName = "type")]
        public AttributeType Type { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "label")]
        public LocalizedString Label { get; private set; }

        [JsonProperty(PropertyName = "inputTip")]
        public LocalizedString InputTip { get; private set; }

        [JsonProperty(PropertyName = "isRequired")]
        public bool? IsRequired { get; private set; }

        [JsonProperty(PropertyName = "attributeConstraint")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AttributeConstraint? AttributeConstraint { get; private set; }

        [JsonProperty(PropertyName = "inputHint")]
        public TextInputHint? InputHint { get; private set; }

        [JsonProperty(PropertyName = "isSearchable")]
        [JsonConverter(typeof(StringEnumConverter))]
        public bool? IsSearchable { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public AttributeDefinition(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            AttributeConstraint attributeConstraint;
            TextInputHint inputHint;

            string attributeConstraintStr = (data.attributeConstraint != null ? data.attributeConstraint.ToString() : string.Empty);
            string inputHintStr = (data.inputHint != null ? data.inputHint.ToString() : string.Empty);

            this.Type = AttributeTypeFactory.Create(data.type);
            this.Name = data.name;
            this.Label = new LocalizedString(data.label);
            this.InputTip = new LocalizedString(data.inputTip);
            this.IsRequired = data.isRequired;
            this.AttributeConstraint = Enum.TryParse(attributeConstraintStr, out attributeConstraint) ? (AttributeConstraint?)attributeConstraint : null;
            this.InputHint = Enum.TryParse(inputHintStr, out inputHint) ? (TextInputHint?)inputHint : null;
            this.IsSearchable = data.isSearchable;
        }

        #endregion
    }
}
