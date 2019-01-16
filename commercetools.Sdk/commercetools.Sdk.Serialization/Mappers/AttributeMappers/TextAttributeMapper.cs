using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    internal class TextAttributeMapper : StringConverter<Attribute, string>, ICustomJsonMapper<Attribute>
    {
    }
}