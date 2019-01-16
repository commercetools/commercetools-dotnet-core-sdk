using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    internal class MoneyAttributeMapper : MoneyConverter<Attribute, BaseMoney>, ICustomJsonMapper<Attribute>
    {
    }
}