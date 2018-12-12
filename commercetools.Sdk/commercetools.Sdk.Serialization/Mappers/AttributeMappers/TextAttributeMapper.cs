using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    public class TextAttributeMapper : StringConverter<Attribute, string>, ICustomJsonMapper<Attribute>
    {
    }
}