using System;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetDateTimeAttribute : SetAttribute<DateTime>
    {
        public SetDateTimeAttribute()
        {
        }
        public SetDateTimeAttribute(SetAttribute<DateTime> attribute)
        {
            CloneProperties(attribute);
        }
    }
}