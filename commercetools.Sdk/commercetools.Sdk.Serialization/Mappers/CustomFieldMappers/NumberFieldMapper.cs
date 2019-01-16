using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class NumberFieldMapper : NumberConverter<Fields, double>, ICustomJsonMapper<Fields>
    {
    }
}