using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain.Products.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Serialization.JsonConverters
{
    public class SimpleAttributeConverterReader : IAttributeConverterReader
    {
        public SimpleAttributeConverterReader()
        {
        }
        
        public object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken valueProperty = jsonObject["value"];
            JToken nameProperty = jsonObject["name"];
            
            return new SimpleAttribute() { JsonValue = valueProperty, Name = nameProperty.Value<string>()};
        }
    }
}