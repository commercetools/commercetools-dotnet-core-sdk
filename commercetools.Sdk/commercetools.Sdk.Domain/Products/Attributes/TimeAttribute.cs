using System;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class TimeAttribute : Attribute<TimeSpan>
    {
        public TimeAttribute()
        {
        }
        public TimeAttribute(Attribute<TimeSpan> attribute)
        {
            CloneProperties(attribute);
        }
    }
}