using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Types.FieldTypes
{
    [TypeMarker("Enum")]
    public class EnumFieldType : FieldType
    {
        public List<EnumValue> Values { get; set; }
    }
}