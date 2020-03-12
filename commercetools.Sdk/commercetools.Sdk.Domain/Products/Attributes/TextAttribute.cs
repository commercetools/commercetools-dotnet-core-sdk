namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class TextAttribute : Attribute<string>
    {
        public TextAttribute()
        {
        }
        public TextAttribute(Attribute<string> attribute)
        {
            CloneProperties(attribute);
        }
    }
}