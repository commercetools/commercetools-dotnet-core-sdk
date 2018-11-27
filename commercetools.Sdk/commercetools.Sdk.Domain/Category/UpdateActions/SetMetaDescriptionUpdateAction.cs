using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetMetaDescriptionUpdateAction : UpdateAction<Category>
    {
        public string Action => "setMetaDescription";
        public LocalizedString MetaDescription { get; set; }
    }
}
