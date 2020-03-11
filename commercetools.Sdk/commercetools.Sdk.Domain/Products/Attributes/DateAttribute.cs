using System;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class DateAttribute : Attribute<DateTime>
    {
        public DateAttribute()
        {
        }
        public DateAttribute(Attribute<DateTime> attribute)
        {
            CloneProperties(attribute);
        }
    }
}