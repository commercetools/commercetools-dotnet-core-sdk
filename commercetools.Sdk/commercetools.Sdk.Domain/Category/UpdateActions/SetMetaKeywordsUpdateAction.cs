using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetMetaKeywordsUpdateAction : UpdateAction<Category>
    {
        public string Action => "setMetaKeywords";
        public LocalizedString MetaKeywords { get; set; }
    }
}
