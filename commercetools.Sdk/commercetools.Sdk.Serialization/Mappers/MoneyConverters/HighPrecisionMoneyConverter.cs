using System;
using System.Linq;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class HighPrecisionMoneyConverter : ICustomJsonMapper<BaseMoney>
    {
        public int Priority => 3;

        public Type Type => typeof(HighPrecisionMoney);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                var currencyCodeProperty = property["currencyCode"];
                var typeProperty = property["type"];
                var typeAttribute =
                    typeof(HighPrecisionMoney).GetCustomAttributes(typeof(MoneyTypeAttribute), false).First() as
                        MoneyTypeAttribute;
                if (currencyCodeProperty != null && typeProperty != null && typeProperty.Value<string>() == typeAttribute.Type.GetDescription())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
