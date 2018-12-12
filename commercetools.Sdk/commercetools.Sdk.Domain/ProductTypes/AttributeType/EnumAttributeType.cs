using System.Collections.Generic;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("enum")]
    public class EnumAttributeType : AttributeType
    {
        public List<PlainEnumValue> Values { get; set; }
    }
}