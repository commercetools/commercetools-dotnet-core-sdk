using System;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Types
{
    /// <summary>
    /// Field definitions describe custom fields and allow you to define some meta-information associated with the field.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-types.html#fielddefinition"/>
    public class FieldDefinition
    {
        #region Properties

        [JsonProperty(PropertyName = "type")]
        public FieldType Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public LocalizedString Label { get; set; }

        [JsonProperty(PropertyName = "required")]
        public bool? Required { get; set; }

        [JsonProperty(PropertyName = "inputHint")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TextInputHint? InputHint { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public FieldDefinition(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            TextInputHint? inputHint;

            this.Type = FieldTypeFactory.Create(data.type);
            this.Name = data.name;
            this.Label = new LocalizedString(data.label);
            this.Required = data.required;
            this.InputHint = Helper.TryGetEnumByEnumMemberAttribute<TextInputHint?>((string)data.inputHint, out inputHint) ? inputHint : null;
        }

        #endregion
    }
}