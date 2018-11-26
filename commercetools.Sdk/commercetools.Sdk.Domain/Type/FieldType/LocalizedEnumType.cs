using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [FieldType("LocalizedEnum")]
    public class LocalizedEnumType : FieldType
    {
        public List<LocalizedEnumValue> Values { get; set; }
    }
}