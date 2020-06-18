namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetEnumAttribute : SetAttribute<PlainEnumValue>
    {
        public SetEnumAttribute()
        {
        }
        public SetEnumAttribute(Attribute<AttributeSet<PlainEnumValue>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}