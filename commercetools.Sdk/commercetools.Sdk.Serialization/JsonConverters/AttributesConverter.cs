using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Serialization.JsonConverters
{
        internal class AttributesConverter : JsonConverterBase
        {
            private DateAttributeMapper dateAttributeMapper;
            private TimeAttributeMapper timeAttributeMapper;
            private DateTimeAttributeMapper dateTimeAttributeMapper;
            private ISerializationConfiguration serializationConfig;

            public AttributesConverter(ISerializationConfiguration serializationConfiguration)
            {
                this.serializationConfig = serializationConfiguration;
                this.dateTimeAttributeMapper = new DateTimeAttributeMapper();
                this.dateAttributeMapper = new DateAttributeMapper();
                this.timeAttributeMapper = new TimeAttributeMapper();
            }

            public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

            public override bool CanConvert(Type objectType)
            {
                if (typeof(List<Attribute>).IsAssignableFrom(objectType))
                {
                    return true;
                }
                return false;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JArray values = JArray.Load(reader);
                return values.Select(token => token as JObject).Select(token =>
                {
                    if (token == null)
                    {
                        return null;
                    }

                    var value = token["value"];
                    if (value == null)
                    {
                        return token.ToObject<Attribute>(serializer);
                    }

                    return token.ToObject(GetAttributeType(value), serializer) as Attribute;
                }).ToList();
            }

            private Type GetAttributeType(JToken value)
            {
                switch (value.Type)
                    {
                        case JTokenType.Boolean:
                            return typeof(BooleanAttribute);
                        case JTokenType.Float:
                        case JTokenType.Integer:
                            return typeof(NumberAttribute);
                        case JTokenType.Date:
                            return typeof(DateTimeAttribute);
                        case JTokenType.TimeSpan:
                            return typeof(TimeAttribute);
                        case JTokenType.String:
                        {
                            if (!serializationConfig.DeserializeDateTimeAttributesAsString && dateTimeAttributeMapper.CanConvert(value))
                                return typeof(DateTimeAttribute);
                            if (!serializationConfig.DeserializeDateAttributesAsString && dateAttributeMapper.CanConvert(value))
                                return typeof(DateAttribute);
                            return timeAttributeMapper.CanConvert(value) ? typeof(TimeAttribute) : typeof(TextAttribute);
                        }
                        case JTokenType.Object:
                            if (!(value is JObject obj))
                            {
                                return typeof(Attribute);
                            }
                            if (obj["typeId"] != null)
                            {
                                return typeof(ReferenceAttribute);
                            }
                            if (obj["currencyCode"] != null)
                            {
                                return typeof(MoneyAttribute);
                            }
                            if (obj["key"] != null)
                            {
                                if (obj["label"]?.Type == JTokenType.String)
                                    return typeof(EnumAttribute);
                                if (obj["label"]?.Type == JTokenType.Object)
                                    return typeof(LocalizedEnumAttribute);
                                return typeof(Attribute);
                            }

                            return typeof(LocalizedTextAttribute);
                        case JTokenType.Array:
                            switch (value.First)
                            {
                                case JArray _:
                                    return typeof(Attribute);
                                case JObject firstValue when firstValue["value"] != null:
                                    return typeof(NestedAttribute);
                                case JObject firstValue:
                                {
                                    var elemType = GetAttributeType(firstValue);
                                    if (elemType == typeof(BooleanAttribute))
                                    {
                                        return typeof(SetBooleanAttribute);
                                    }
                                    if (elemType == typeof(NumberAttribute))
                                    {
                                        return typeof(SetNumberAttribute);
                                    }
                                    if (elemType == typeof(DateTimeAttribute))
                                    {
                                        return typeof(SetDateTimeAttribute);
                                    }
                                    if (elemType == typeof(DateAttribute))
                                    {
                                        return typeof(SetDateTimeAttribute);
                                    }
                                    if (elemType == typeof(TimeAttribute))
                                    {
                                        return typeof(SetTimeAttribute);
                                    }
                                    if (elemType == typeof(TextAttribute))
                                    {
                                        return typeof(SetTextAttribute);
                                    }
                                    if (elemType == typeof(ReferenceAttribute))
                                    {
                                        return typeof(SetReferenceAttribute);
                                    }
                                    if (elemType == typeof(MoneyAttribute))
                                    {
                                        return typeof(SetMoneyAttribute);
                                    }
                                    if (elemType == typeof(EnumAttribute))
                                    {
                                        return typeof(SetEnumAttribute);
                                    }
                                    if (elemType == typeof(LocalizedEnumAttribute))
                                    {
                                        return typeof(SetLocalizedEnumAttribute);
                                    }
                                    if (elemType == typeof(LocalizedTextAttribute))
                                    {
                                        return typeof(SetLocalizedTextAttribute);
                                    }
                                    return typeof(Attribute);
                                }
                                    
                                default:
                                    return typeof(Attribute);
                            }

                        default: return typeof(Attribute);
                    }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
}