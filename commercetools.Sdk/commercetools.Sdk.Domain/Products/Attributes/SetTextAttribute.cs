namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetTextAttribute : SetAttribute<string>
    {
        public SetTextAttribute()
        {
        }
        public SetTextAttribute(Attribute<AttributeSet<string>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}