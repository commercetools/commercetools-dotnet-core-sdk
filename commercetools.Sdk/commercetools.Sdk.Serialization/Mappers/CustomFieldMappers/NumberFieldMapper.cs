using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class NumberFieldMapper : NumberConverter<Fields, double>, ICustomJsonMapper<Fields>
    {
    }
}