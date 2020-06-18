namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetNestedAttribute : SetAttribute<NestedAttribute>
    {
        public SetNestedAttribute()
        {
        }
        public SetNestedAttribute(Attribute<AttributeSet<NestedAttribute>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}