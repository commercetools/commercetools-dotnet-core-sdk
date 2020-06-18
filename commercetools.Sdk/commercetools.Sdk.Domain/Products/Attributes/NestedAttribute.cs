using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class NestedAttribute : Attribute<List<Attribute>>
    {
        public NestedAttribute()
        {
        }
        public NestedAttribute(Attribute<List<Attribute>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}