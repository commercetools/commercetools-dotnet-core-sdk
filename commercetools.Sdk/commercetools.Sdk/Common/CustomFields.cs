using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class CustomFields
    {
        public Reference<Type> Type { get; set; }
        public FieldCollection Fields { get; set; }
    }
}