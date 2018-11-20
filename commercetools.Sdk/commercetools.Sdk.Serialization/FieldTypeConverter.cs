using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class FieldTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(FieldType))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken nameProperty = jsonObject["name"];
            
            Type fieldType = GetTypeByCodeProperty(nameProperty);

            if (fieldType == null)
            {
                throw new JsonSerializationException("Field type cannot be determined.");
            }

            return jsonObject.ToObject(fieldType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }   

        private Type GetTypeByCodeProperty(JToken nameProperty)
        {
            var derivedFieldTypes = typeof(FieldType).GetAllDerivedClassTypesForClass(Assembly.GetAssembly(typeof(FieldType)));
            foreach(Type type in derivedFieldTypes)
            {
                FieldInfo field = type.GetField("NAME", BindingFlags.Public | BindingFlags.Static);
                string name = (string)field.GetValue(null);
                if (!string.IsNullOrEmpty(name))
                {
                    if (name == nameProperty.Value<string>())
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        
    }
}