namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetLocalizedEnumAttribute : SetAttribute<LocalizedEnumValue>
    {
        public SetLocalizedEnumAttribute()
        {
        }
        public SetLocalizedEnumAttribute(SetAttribute<LocalizedEnumValue> attribute)
        {
            CloneProperties(attribute);
        }
    }
}