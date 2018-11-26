using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    [FieldType("Reference")]
    public class ReferenceType : FieldType
    {
        public ReferenceTypeId ReferenceTypeId { get; set; }
    }
}
