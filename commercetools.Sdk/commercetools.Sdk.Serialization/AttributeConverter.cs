using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization
{
    public class AttributeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        // TODO Refactor
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var j = JObject.Load(reader);
            var valueProperty = j["value"];
            if (valueProperty == null)
            {
                // TODO Throw a custom exception perhaps
                throw new Exception();
            }
            if (valueProperty.HasValues)
            {
                var keyProperty = valueProperty["key"];
                if (keyProperty != null)
                {
                    Type resultType = typeof(EnumAttribute);
                    return j.ToObject(resultType);
                }
                // TODO Add support for Money derived classes
                var currencyCodeProperty = valueProperty["currencyCode"];
                if (currencyCodeProperty != null)
                {
                    Type resultType = typeof(MoneyAttribute);
                    return j.ToObject(resultType);
                }
            }
            else
            {
                // simple types
                if (valueProperty.Type == JTokenType.String)
                {
                    Type resultType = typeof(TextAttribute);
                    DateTime time;
                    if (DateTime.TryParse(valueProperty.Value<string>(), out time))
                    {
                        if (time.TimeOfDay.Ticks == 0)
                        {
                            resultType = typeof(DateAttribute);
                        }
                        else
                        { 
                            resultType = typeof(TimeAttribute);
                        }
                    }
                    return j.ToObject(resultType);
                }
                if (valueProperty.Type == JTokenType.Date)
                {
                    Type resultType = typeof(DateTimeAttribute);
                    return j.ToObject(resultType);
                }
                if (valueProperty.Type == JTokenType.Integer)
                {
                    Type resultType = typeof(NumberAttribute);
                    return j.ToObject(resultType);
                }
                if (valueProperty.Type == JTokenType.Boolean)
                {
                    Type resultType = typeof(BooleanAttribute);
                    return j.ToObject(resultType);
                }
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
