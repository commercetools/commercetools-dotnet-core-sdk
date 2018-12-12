using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    public class EnumAttributeMapper : EnumConverter<Attribute, PlainEnumValue>, ICustomJsonMapper<Attribute>
    {
    }
}