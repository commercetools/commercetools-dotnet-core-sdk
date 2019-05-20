using System;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public static class Extensions
    {
        public static double NextDouble(this Random rnd, double min, double max)
        {
            return rnd.NextDouble() * (max-min) + min;
        }

        public static string GetTextAttributeValue(this ProductVariant variant, string textAttributeName)
        {
            string attributeValue = null;
            var attribute = variant.Attributes.FirstOrDefault(a => a.Name.Equals(textAttributeName));
            if (attribute != null)//if there is attribute with name = textAttributeName
            {
                attributeValue = (attribute as Attribute<string>)?.Value;
            }
            return attributeValue;
        }
    }
}
