using System;
using commercetools.Sdk.Domain.CustomObject;
using commercetools.Sdk.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.HttpApi.Extensions
{
    public static class CustomObjectExtension
    {
        public static ISerializerService Serializer { get; set; }

        public static object GetValueAs(this CustomObject customObject, System.Type type)
        {
            if (customObject.Value is JToken token)
            {
                return token.ToObject(type);
            }

            throw new ArgumentException("Value is not a JSON object");
        }

        public static object GetValueAs(this CustomObject customObject, System.Type type, JsonSerializer jsonSerializer)
        {
            if (customObject.Value is JToken token)
            {
                return token.ToObject(type, jsonSerializer);
            }

            throw new ArgumentException("Value is not a JSON object");
        }

        public static T GetValueAs<T>(this CustomObject customObject)
        {
            return (T)GetValueAs(customObject, typeof(T));
        }

        public static T GetValueAs<T>(this CustomObject customObject, JsonSerializer jsonSerializer)
        {
            return (T)GetValueAs(customObject, typeof(T), jsonSerializer);
        }
    }
}
