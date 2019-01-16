using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    internal class BooleanAttributeMapper : BooleanConverter<Attribute, bool>, ICustomJsonMapper<Attribute>
    {
    }
}