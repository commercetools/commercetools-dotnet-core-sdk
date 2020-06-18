namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetBooleanAttribute : SetAttribute<bool>
    {
        public SetBooleanAttribute()
        {
        }
        public SetBooleanAttribute(Attribute<AttributeSet<bool>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}