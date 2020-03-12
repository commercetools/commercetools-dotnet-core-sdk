namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetReferenceAttribute : SetAttribute<Reference>
    {
        public SetReferenceAttribute()
        {
        }
        public SetReferenceAttribute(SetAttribute<Reference> attribute)
        {
            CloneProperties(attribute);
        }
    }
}