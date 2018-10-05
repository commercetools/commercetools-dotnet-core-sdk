using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Serialization
{
    public class AttributeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        // TODO Refactor; try to reduce branches
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken valueProperty = jsonObject["value"];
            if (valueProperty == null)
            {
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("Property value is not present in the attribute object.");
            }
            Type attributeType;
            if (IsSetAttribute(valueProperty))
            {
                attributeType = GetSetTypeByValueProperty(valueProperty);
            }
            else
            {
                attributeType = GetTypeByValueProperty(valueProperty);
            }

            // TODO Attribute is never null, exceptions are always thrown, refactor perhaps
            if (attributeType == null)
            {
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("Attribute type cannot be determined.");
            }
            return jsonObject.ToObject(attributeType);
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
            if (valueProperty.HasValues)
            {
                var keyProperty = valueProperty["key"];
                if (keyProperty != null)
                {
                    var labelProperty = valueProperty["label"];
                    if (labelProperty == null)
                    {
                        // TODO Move this message to a localizable resource and add more information to the exception
                        throw new JsonSerializationException("There is no label property next to the key property.");
                    }
                    if (labelProperty.HasValues)
                    {
                        return typeof(LocalizedEnumAttribute);
                    }
                    return typeof(EnumAttribute);
                }
                // TODO Add support for Money derived classes
                var currencyCodeProperty = valueProperty["currencyCode"];
                if (currencyCodeProperty != null)
                {
                    return typeof(MoneyAttribute);
                }
                return typeof(LocalizedTextAttribute);
            }
            else
            {
                if (valueProperty.Type == JTokenType.String)
                {
                    DateTime time;
                    if (DateTime.TryParse(valueProperty.Value<string>(), out time))
                    {
                        if (time.TimeOfDay.Ticks == 0)
                        {
                            return typeof(DateAttribute);
                        }
                        else
                        {
                            return typeof(TimeAttribute);
                        }
                    }
                    return typeof(TextAttribute);
                }
                if (valueProperty.Type == JTokenType.Date)
                {
                    return typeof(DateTimeAttribute);
                }
                if (valueProperty.Type == JTokenType.Integer)
                {
                    return typeof(NumberAttribute);
                }
                if (valueProperty.Type == JTokenType.Boolean)
                {
                    return typeof(BooleanAttribute);
                }
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("The structure does not match any of the attribute subtypes.");
            }
        }

        private bool IsSetAttribute(JToken valueProperty)
        {
            return valueProperty.HasValues && valueProperty.Type == JTokenType.Array;
        }
    }
}