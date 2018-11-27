using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetCustomFieldUpdateAction : UpdateAction<Category>
    {
        public string Action => "setCustomField";
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
