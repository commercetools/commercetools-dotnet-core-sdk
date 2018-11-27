using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    [FieldType("Reference")]
    public class ReferenceFieldType : FieldType
    {
        public ReferenceFieldTypeId ReferenceTypeId { get; set; }
    }
}
