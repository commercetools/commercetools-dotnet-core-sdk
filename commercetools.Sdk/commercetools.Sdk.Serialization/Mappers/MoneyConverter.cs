using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class MoneyConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 3;

        public Type Type => typeof(S);

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