namespace commercetools.Sdk.Domain
{
    using commercetools.Sdk.Domain.Attributes;
    using System.Collections.Generic;

    [TypeMarker("enum")]
    public class EnumAttributeType : AttributeType
    {
        public List<PlainEnumValue> Values { get; set; }
    }
}