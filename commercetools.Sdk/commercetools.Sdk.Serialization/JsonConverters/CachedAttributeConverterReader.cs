using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Type = System.Type;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization.JsonConverters
{
    public class CachedAttributeConverterReader : IAttributeConverterReader
    {
        private readonly IMapperTypeRetriever<Attribute> mapperTypeRetriever;
        private ConcurrentDictionary<string, Type> attributeTypes = new ConcurrentDictionary<string, Type>();

        public CachedAttributeConverterReader(IMapperTypeRetriever<Attribute> mapperTypeRetriever)
        {
            this.mapperTypeRetriever = mapperTypeRetriever;
        }

        public object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken valueProperty = jsonObject["value"];
            JToken nameProperty = jsonObject["name"];
            string nameValue = nameProperty.Value<string>() + (valueProperty?.Type ?? JTokenType.None);
            object attrObject = null;
            
            if (valueProperty.IsNestedAttribute())
            {
                attrObject = jsonObject.ToObject(typeof(NestedAttribute), serializer);
            }
            else if(valueProperty.IsSetOfNestedAttribute())
            {
                attrObject = ConvertToSetOfNestedAttribute(jsonObject, serializer);
            }
            else
            {
               
                Type attributeType = this.attributeTypes.GetOrAdd(nameValue, (obj) => {
                    Type genericType = this.mapperTypeRetriever.GetTypeForToken(valueProperty);
                
                    if (genericType == null)
                    {
                        throw new JsonSerializationException($"Couldn't deserialize attribute '{nameProperty}' value: '{valueProperty?.ToString(Formatting.None)}'");
                    }
                    return typeof(Attribute<>).MakeGenericType(genericType);
                });
                attrObject = jsonObject.ToObject(attributeType, serializer);    
            }
            
            if (attrObject is IAttribute attr)
            {
                attr.JsonValue = valueProperty;
            }
            return attrObject;
        }
        
        private Attribute<AttributeSet<NestedAttribute>> ConvertToSetOfNestedAttribute(
                    JToken jsonObject, JsonSerializer serializer)
        {
            var valueProperty = jsonObject["value"];
            var nameProperty = jsonObject["name"];
            
            var setAttr = new Attribute<AttributeSet<NestedAttribute>>();
            var setOfNestedAttributes = new AttributeSet<NestedAttribute>();
            foreach(var item in valueProperty.Children())
            {
                var attArr = item;
                if (item.IsNestedAttributeObject())
                {
                    attArr = item.First.Children().First();
                }
                var listOfAttrs = attArr.ToObject(typeof(List<Attribute>), serializer);
                var nestedAttr = new NestedAttribute(new Attribute<List<Attribute>>()
                {
                    Value = listOfAttrs as List<Attribute>,
                    JsonValue = attArr
                });
                setOfNestedAttributes.Add(nestedAttr);
            }

            setAttr.Value = setOfNestedAttributes;
            setAttr.Name = nameProperty.ToString();
            return setAttr;
        }
    }
}