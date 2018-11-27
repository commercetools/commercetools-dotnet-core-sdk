using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class BooleanFieldMapper : BooleanConverter<Fields, bool>, ICustomJsonMapper<Fields>
    {
    }
}