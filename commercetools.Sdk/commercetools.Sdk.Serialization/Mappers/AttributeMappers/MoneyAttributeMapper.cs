using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class MoneyAttributeMapper : MoneyConverter<Domain.Attribute, Money>, ICustomJsonMapper<Domain.Attribute>
    {
    }
}