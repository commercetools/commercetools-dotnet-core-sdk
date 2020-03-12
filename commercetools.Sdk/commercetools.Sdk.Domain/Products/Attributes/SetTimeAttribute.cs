using System;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetTimeAttribute : SetAttribute<TimeSpan>
    {
        public SetTimeAttribute()
        {
        }
        public SetTimeAttribute(SetAttribute<TimeSpan> attribute)
        {
            CloneProperties(attribute);
        }
    }
}