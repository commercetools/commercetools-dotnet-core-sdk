namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class ReferenceAttribute : Attribute<Reference>
    {
        public ReferenceAttribute()
        {
        }
        public ReferenceAttribute(Attribute<Reference> attribute)
        {
            CloneProperties(attribute);
        }
    }
}