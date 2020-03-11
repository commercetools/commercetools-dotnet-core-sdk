using System;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class DateTimeAttribute : Attribute<DateTime>
    {
        public DateTimeAttribute()
        {
        }
        public DateTimeAttribute(Attribute<DateTime> attribute)
        {
            CloneProperties(attribute);
        }
    }
}