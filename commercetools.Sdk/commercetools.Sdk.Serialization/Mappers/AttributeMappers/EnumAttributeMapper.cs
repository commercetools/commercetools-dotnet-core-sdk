using commercetools.Sdk.Domain.Attributes;

namespace commercetools.Sdk.Serialization
{
    public class EnumAttributeMapper : EnumConverter<Domain.Attribute, PlainEnumValue>, ICustomJsonMapper<Domain.Attribute>
    {
    }
}