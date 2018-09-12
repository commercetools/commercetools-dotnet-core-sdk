using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class CustomFields
    {
        public Reference Type { get; set; }
        public List<FieldDefinition> Fields { get; set; }
    }
}