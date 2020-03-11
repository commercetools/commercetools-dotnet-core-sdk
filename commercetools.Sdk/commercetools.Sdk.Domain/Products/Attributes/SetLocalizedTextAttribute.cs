namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetLocalizedTextAttribute : SetAttribute<LocalizedString>
    {
        public SetLocalizedTextAttribute()
        {
        }
        public SetLocalizedTextAttribute(SetAttribute<LocalizedString> attribute)
        {
            CloneProperties(attribute);
        }
    }
}