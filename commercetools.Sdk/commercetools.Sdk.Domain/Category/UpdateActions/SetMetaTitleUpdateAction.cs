using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetMetaTitleUpdateAction : UpdateAction<Category>
    {
        public string Action => "setMetaTitle";
        public LocalizedString MetaTitle { get; set; }
    }
}
