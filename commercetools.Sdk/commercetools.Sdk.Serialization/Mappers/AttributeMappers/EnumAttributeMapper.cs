using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    internal class EnumAttributeMapper : EnumConverter<Attribute, PlainEnumValue>, ICustomJsonMapper<Attribute>
    {
    }
}