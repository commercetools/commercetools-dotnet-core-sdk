using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class SetKeyUpdateAction : UpdateAction<Category>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}
