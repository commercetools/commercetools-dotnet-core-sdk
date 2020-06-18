using System.Collections.Generic;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Serialization
{
    internal class NestedAttributeMapper : NestedConverter<Attribute, List<Attribute>>, ICustomJsonMapper<Attribute>
    {
    }
}
