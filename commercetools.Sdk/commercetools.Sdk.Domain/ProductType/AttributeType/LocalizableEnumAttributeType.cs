namespace commercetools.Sdk.Domain
{
    using System.Collections.Generic;

    [TypeMarker("lenum")]
    public class LocalizableEnumAttributeType : AttributeType
    {
        public List<Attributes.LocalizedEnumValue> Values { get; set; }
    }
}