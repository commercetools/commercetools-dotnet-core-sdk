namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetEnumAttribute : SetAttribute<PlainEnumValue>
    {
        public SetEnumAttribute()
        {
        }
        public SetEnumAttribute(SetAttribute<PlainEnumValue> attribute)
        {
            CloneProperties(attribute);
        }
    }
}