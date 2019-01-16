using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class StringFieldMapper : StringConverter<Fields, string>, ICustomJsonMapper<Fields>
    {
    }
}