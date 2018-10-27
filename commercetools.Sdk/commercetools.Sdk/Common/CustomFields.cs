using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class CustomFields
    {
        public Reference<Type> Type { get; set; }
        public IFields Fields { get; set; }
    }
}