using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class StringFieldMapper : StringConverter<Fields, string>, ICustomJsonMapper<Fields>
    {
    }
}