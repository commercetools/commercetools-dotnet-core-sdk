using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class BooleanFieldMapper : BooleanConverter<Fields, bool>, ICustomJsonMapper<Fields>
    {
    }
}