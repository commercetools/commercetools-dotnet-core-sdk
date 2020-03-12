using System;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetDateAttribute : SetAttribute<DateTime>
    {
        public SetDateAttribute()
        {
            
        }
        public SetDateAttribute(SetAttribute<DateTime> attribute)
        {
            CloneProperties(attribute);
        }
    }
}