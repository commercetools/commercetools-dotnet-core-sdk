using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("LocalizedEnum")]
    public class LocalizedEnumFieldType : FieldType
    {
        public List<LocalizedEnumValue> Values { get; set; }
    }
}