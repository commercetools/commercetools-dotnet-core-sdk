using System;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetTimeAttribute : SetAttribute<TimeSpan>
    {
        public SetTimeAttribute()
        {
        }
        public SetTimeAttribute(Attribute<AttributeSet<TimeSpan>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}