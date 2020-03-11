namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class LocalizedTextAttribute : Attribute<LocalizedString>
    {
        public LocalizedTextAttribute()
        {
            
        }
        public LocalizedTextAttribute(Attribute<LocalizedString> attribute)
        {
            CloneProperties(attribute);
        }
    }
}