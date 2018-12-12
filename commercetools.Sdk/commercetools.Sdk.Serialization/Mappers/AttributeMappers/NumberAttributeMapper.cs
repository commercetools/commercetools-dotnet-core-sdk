using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    public class NumberAttributeMapper : NumberConverter<Attribute, double>, ICustomJsonMapper<Attribute>
    {
    }
}