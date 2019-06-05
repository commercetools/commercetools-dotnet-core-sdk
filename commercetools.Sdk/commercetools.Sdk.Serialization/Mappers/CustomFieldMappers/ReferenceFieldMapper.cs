using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    internal class ReferenceFieldMapper : ReferenceConverter<Fields, Reference>, ICustomJsonMapper<Fields>
    {
    }
}
