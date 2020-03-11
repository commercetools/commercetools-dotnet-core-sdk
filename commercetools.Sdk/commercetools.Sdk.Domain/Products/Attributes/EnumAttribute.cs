namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class EnumAttribute : Attribute<PlainEnumValue>
    {
        public EnumAttribute()
        {
        }
        public EnumAttribute(Attribute<PlainEnumValue> attribute)
        {
            CloneProperties(attribute);
        }
    }
}