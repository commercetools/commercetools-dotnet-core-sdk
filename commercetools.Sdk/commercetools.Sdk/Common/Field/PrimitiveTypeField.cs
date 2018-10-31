using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class PrimitiveTypeField<T> : IField
    {
        public T Value { get; set; }
    }
}
