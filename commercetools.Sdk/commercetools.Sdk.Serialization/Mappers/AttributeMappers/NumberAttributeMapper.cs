using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    internal class NumberAttributeMapper : NumberConverter<Attribute, double>, ICustomJsonMapper<Attribute>
    {
    }
}