namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetLocalizedTextAttribute : SetAttribute<LocalizedString>
    {
        public SetLocalizedTextAttribute()
        {
        }
        public SetLocalizedTextAttribute(Attribute<AttributeSet<LocalizedString>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}