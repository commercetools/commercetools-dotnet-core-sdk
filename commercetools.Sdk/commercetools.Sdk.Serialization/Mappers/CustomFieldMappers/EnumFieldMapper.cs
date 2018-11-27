using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class EnumFieldMapper : EnumConverter<Fields, EnumValue>, ICustomJsonMapper<Fields>
    {
    }
}