namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetLocalizedEnumAttribute : SetAttribute<LocalizedEnumValue>
    {
        public SetLocalizedEnumAttribute()
        {
        }
        public SetLocalizedEnumAttribute(Attribute<AttributeSet<LocalizedEnumValue>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}