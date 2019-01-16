using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    internal class ReferenceAttributeMapper : ReferenceConverter<Attribute, Reference>, ICustomJsonMapper<Attribute>
    {
    }
}