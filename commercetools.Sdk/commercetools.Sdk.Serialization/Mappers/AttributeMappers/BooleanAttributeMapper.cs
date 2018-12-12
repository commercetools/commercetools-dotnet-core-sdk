using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    public class BooleanAttributeMapper : BooleanConverter<Attribute, bool>, ICustomJsonMapper<Attribute>
    {
    }
}