using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class MoneyFieldMapper : MoneyConverter<Fields, Money>, ICustomJsonMapper<Fields>
    {
    }
}