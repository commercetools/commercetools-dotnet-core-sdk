namespace commercetools.Sdk.Domain
{
    using commercetools.Sdk.Domain.Products.Attributes;
    using System.Collections.Generic;

    [TypeMarker("lenum")]
    public class LocalizableEnumAttributeType : AttributeType
    {
        public List<LocalizedEnumValue> Values { get; set; }
    }
}