namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetReferenceAttribute : SetAttribute<Reference>
    {
        public SetReferenceAttribute()
        {
        }
        public SetReferenceAttribute(Attribute<AttributeSet<Reference>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}