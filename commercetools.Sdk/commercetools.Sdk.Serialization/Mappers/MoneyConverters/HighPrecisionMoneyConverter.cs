using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class HighPrecisionMoneyConverter : ICustomJsonMapper<BaseMoney>
    {
        public int Priority => 3;

        public Type Type => typeof(HighPrecisionMoney);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                var currencyCodeProperty = property["currencyCode"];
                var typeProperty = property["type"];
                // TODO Take highPrecision string from annotation attribute from HighPrecisionMoney
                if (currencyCodeProperty != null && typeProperty != null && typeProperty.Value<string>() == "highPrecision")
                {
                    return true;
                }
            }
            return false;
        }
    }
}