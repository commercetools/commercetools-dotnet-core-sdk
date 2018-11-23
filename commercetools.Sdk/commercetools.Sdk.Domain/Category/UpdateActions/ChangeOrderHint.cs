using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class ChangeOrderHint : UpdateAction<Category>
    {
        public string Action => "changeOrderHint";
        public string OrderHint { get; set; }
    }
}
