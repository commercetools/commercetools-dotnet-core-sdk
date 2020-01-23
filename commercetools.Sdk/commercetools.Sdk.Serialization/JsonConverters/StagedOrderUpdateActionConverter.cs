using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.OrderEdits;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Registration;
using Newtonsoft.Json.Linq;
using Type = System.Type;
namespace commercetools.Sdk.Serialization.JsonConverters
{
    public class StagedOrderUpdateActionConverter : JsonConverterBase
    {
        private readonly IEnumerable<Type> derivedTypes;
        public string PropertyName => "Action";

        public StagedOrderUpdateActionConverter(ITypeRetriever typeRetriever)
        {
            this.derivedTypes = typeRetriever.GetTypes<StagedOrderUpdateAction>();
        }
        
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(StagedOrderUpdateAction);
        }

        public override List<SerializerType> SerializerTypes => new List<SerializerType>()
            {SerializerType.Deserialization};

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken jsonProp = jsonObject[PropertyName.ToCamelCase()];
            var jsonPropValue = jsonProp.Value<string>();
            Type actionType = null;
            
            foreach (Type type in this.derivedTypes)
            {
                var instance = Activator.CreateInstance(type);
                var actionProp = type.GetProperty(PropertyName);
                var actionPropValue = actionProp?.GetValue(instance).ToString();
                if (actionProp != null && actionPropValue.Equals(jsonPropValue))
                {
                    actionType = type;
                }
            }
            
            if (actionType != null)
            {
                return jsonObject.ToObject(actionType, serializer);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}