using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class EnumFieldMapper : EnumConverter<Fields, EnumValue>, ICustomJsonMapper<Fields>
    {
    }
}