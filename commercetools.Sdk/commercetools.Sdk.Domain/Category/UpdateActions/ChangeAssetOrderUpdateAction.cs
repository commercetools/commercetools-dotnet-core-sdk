using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Categories
{
    public class ChangeAssetOrderUpdateAction : UpdateAction<Category>
    {
        public string Action => "changeAssetOrder";
        public List<string> AsserOrder { get; set; }
    }
}
