using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class MoneyFieldMapper : MoneyConverter<Fields, BaseMoney>, ICustomJsonMapper<Fields>
    {
    }
}