using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization
{
    public class MoneyAttributeConverter : ICustomConverter<Domain.Attribute>
    {
        public int Priority => 3;

        public Type Type => typeof(MoneyAttribute);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                var currencyCodeProperty = property["currencyCode"];
                if (currencyCodeProperty != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
