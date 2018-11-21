using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [FieldType("Enum")]
    public class EnumType : FieldType
    {
        public List<EnumValue> Values { get; set; }
    }
}