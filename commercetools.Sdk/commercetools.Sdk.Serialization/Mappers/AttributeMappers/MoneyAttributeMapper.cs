using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class MoneyAttributeMapper : MoneyConverter<Domain.Attribute, BaseMoney>, ICustomJsonMapper<Domain.Attribute>
    {
    }
}