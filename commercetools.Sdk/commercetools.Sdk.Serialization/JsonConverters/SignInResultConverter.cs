using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;
using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Serialization
{
    public class SignInResultConverter : JsonConverterBase
    {
        public override List<SerializerType> SerializerTypes => new List<SerializerType>() { SerializerType.Deserialization };

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(SignInResult<Customer>))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            return jsonObject.ToObject(typeof(CustomerSignInResult), serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}