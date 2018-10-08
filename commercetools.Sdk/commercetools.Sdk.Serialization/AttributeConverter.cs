using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.Serialization
{
    public class AttributeConverter : JsonConverter
    {
        private readonly IEnumerable<ICustomConverter<Domain.Attribute>> customConverters;
        private JsonSerializer moneySerializer;

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public AttributeConverter(IEnumerable<ICustomConverter<Domain.Attribute>> customConverters, MoneyConverter moneyConverter)
        {
            this.customConverters = customConverters;
            JsonSerializer moneySerializer = new JsonSerializer();
            moneySerializer.Converters.Add(moneyConverter);
            this.moneySerializer = moneySerializer;
        }

        // TODO Refactor; try to reduce branches
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken valueProperty = jsonObject["value"];
            Type attributeType;
            if (IsSetAttribute(valueProperty))
            {
                attributeType = GetSetTypeByValueProperty(valueProperty);
            }
            else
            {
                attributeType = GetTypeByValueProperty(valueProperty);
            }

            if (attributeType == null)
            {
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("Attribute type cannot be determined.");
            }

            return jsonObject.ToObject(attributeType, this.moneySerializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private Type GetSetTypeBySimpleType(Type simpleType)
        {
            // TODO Move this to different class and add all types
            Dictionary<Type, Type> setMapping = new Dictionary<Type, Type>();
            setMapping.Add(typeof(TextAttribute), typeof(SetTextAttribute));
            if (setMapping.ContainsKey(simpleType))
            {
                return setMapping[simpleType];
            }
            // TODO Move this message to a localizable resource and add more information to the exception
            throw new JsonSerializationException("There is not set attribute defined for the given structure.");
        }

        private Type GetSetTypeByValueProperty(JToken valueProperty)
        {
            Type simpleType = GetTypeByValueProperty(valueProperty[0]);
            if (simpleType != null)
            {
                return GetSetTypeBySimpleType(simpleType);
            }
            return null;
        }

        private Type GetTypeByValueProperty(JToken valueProperty)
        {
            foreach (var customConvert in this.customConverters.OrderBy(c => c.Priority))
            {
                if (customConvert.CanConvert(valueProperty))
                {
                    return customConvert.Type;
                }
            }
            return null;
        }

        private bool IsSetAttribute(JToken valueProperty)
        {
            if (valueProperty != null)
            { 
                return valueProperty.HasValues && valueProperty.Type == JTokenType.Array;
            }
            return false;
        }
    }
}