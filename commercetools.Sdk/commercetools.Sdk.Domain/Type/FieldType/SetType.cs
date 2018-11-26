using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    [FieldType("Set")]
    public class SetType : FieldType
    {
        public FieldType ElementType { get; set; }
    }
}
