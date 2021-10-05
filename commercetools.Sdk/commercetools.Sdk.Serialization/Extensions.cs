using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.Serialization
{
    public static class Extensions
    {
        public static bool IsSetAttribute(this JToken valueProperty)
        {
            return valueProperty.IsArrayToken();
        }
        public static bool IsSetOfNestedAttribute(this JToken valueProperty)
        {
            var isArrayToken = valueProperty.IsArrayToken();
            var array = valueProperty as JArray;
            if (array == null || !isArrayToken || !array.Children().Any())
                return false;

            var isFirstItemNestedAttribute = array.First.IsNestedAttribute();
            return isFirstItemNestedAttribute;
        }
        
        public static bool IsNestedAttribute(this JToken valueProperty)
        {
            JToken tokenAttr = valueProperty;

            if (valueProperty.IsNestedAttributeObject())
            {
                tokenAttr = valueProperty.First.Children().First();
            }

            var isArrayToken = tokenAttr.IsArrayToken();
            var array = tokenAttr as JArray;
            if (array == null || !isArrayToken || !array.Children().Any())
                return false;
            
            var itemProperties = array.First.Children<JProperty>();
            var nameProp = itemProperties.FirstOrDefault(p => p.Name == "name");
            var valueProp = itemProperties.FirstOrDefault(p => p.Name == "value");
            return itemProperties.Count() == 2 && nameProp != null && valueProp != null;
        }

        public static bool IsNestedAttributeObject(this JToken valueProperty)
        {
            if (valueProperty !=null
                && valueProperty.Type == JTokenType.Object
                && valueProperty.Children().Count() == 1
                && valueProperty.First.Type == JTokenType.Property
                && (valueProperty.First as JProperty)?.Name == "value"
                && valueProperty.First.Children().First().IsArrayToken())
                return true;
            
            return false;
        }

        public static bool IsArrayToken(this JToken valueProperty)
        {
            return valueProperty != null && valueProperty.Type == JTokenType.Array;
        }
        
        public static string ToCamelCase(this string stringValue)
        {
            var result = Char.ToLowerInvariant(stringValue[0])
                         + stringValue.Substring(1);
            return result;
        }
    }
}