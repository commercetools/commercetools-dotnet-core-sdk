using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Common.Field
{
    public class StringField<T> : Field<string>
    {
        public T Value { get; set; }
    }
}
