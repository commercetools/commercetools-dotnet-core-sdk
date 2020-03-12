namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class BooleanAttribute : Attribute<bool>
    {
        public BooleanAttribute()
        {
        }
        public BooleanAttribute(Attribute<bool> attribute)
        {
            CloneProperties(attribute);
        }
    }
}