namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class LocalizedEnumAttribute : Attribute<LocalizedEnumValue>
    {
        public LocalizedEnumAttribute()
        {
        }
        public LocalizedEnumAttribute(Attribute<LocalizedEnumValue> attribute)
        {
            CloneProperties(attribute);
        }
    }
}