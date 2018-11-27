using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetDescriptionUpdateAction : UpdateAction<Category>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}
