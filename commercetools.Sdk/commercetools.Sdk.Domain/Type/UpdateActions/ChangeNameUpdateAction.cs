using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class ChangeNameUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeName"; 
        public LocalizedString Name { get; set; }
    }
}
