namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetBooleanAttribute : SetAttribute<bool>
    {
        public SetBooleanAttribute()
        {
        }
        public SetBooleanAttribute(SetAttribute<bool> attribute)
        {
            CloneProperties(attribute);
        }
    }
}