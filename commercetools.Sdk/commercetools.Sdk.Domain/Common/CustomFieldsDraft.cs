using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class CustomFieldsDraft : IDraft<CustomFields>
    {
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}
