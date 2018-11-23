using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class ChangeKeyUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeKey"; 
        public string Key { get; set; }
    }
}
