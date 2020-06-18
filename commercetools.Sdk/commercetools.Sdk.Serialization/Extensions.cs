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
            if (array == null || !isArrayToken)
                return false;

            var isFirstItemNestedAttribute = array.First.IsNestedAttribute();
            return isFirstItemNestedAttribute;
        }
        
        public static bool IsNestedAttribute(this JToken valueProperty)
        {
            var isArrayToken = valueProperty.IsArrayToken();
            var array = valueProperty as JArray;
            if (array == null || !isArrayToken || !array.Children().Any())
                return false;
            
            var allElementsContainsNameValueProps = true;
            
            foreach(var item in array.Children())
            {
                var itemProperties = item.Children<JProperty>();
                var nameProp = itemProperties.FirstOrDefault(p => p.Name == "name");
                var valueProp = itemProperties.FirstOrDefault(p => p.Name == "value");
                if (itemProperties.Count() != 2 
                    || nameProp == null
                    || valueProp == null)
                {
                    allElementsContainsNameValueProps = false;
                }
            }
            return allElementsContainsNameValueProps;
        }
        
        public static bool IsArrayToken(this JToken valueProperty)
        {
            return valueProperty != null && valueProperty.Type == JTokenType.Array;
        }
    }
}