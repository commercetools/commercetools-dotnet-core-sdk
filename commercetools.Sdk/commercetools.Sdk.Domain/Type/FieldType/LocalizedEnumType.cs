using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("LocalizedEnum")]
    public class LocalizedEnumType : FieldType
    {
        public List<LocalizedEnumValue> Values { get; set; }
    }
}