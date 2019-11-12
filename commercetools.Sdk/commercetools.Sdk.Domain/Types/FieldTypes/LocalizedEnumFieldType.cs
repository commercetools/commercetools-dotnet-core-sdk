using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Types.FieldTypes
{
    [TypeMarker("LocalizedEnum")]
    public class LocalizedEnumFieldType : FieldTypes.FieldType
    {
        public List<LocalizedEnumValue> Values { get; set; }
    }
}