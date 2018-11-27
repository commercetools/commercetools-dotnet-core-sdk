using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetCustomTypeUpdateAction : UpdateAction<Category>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}
