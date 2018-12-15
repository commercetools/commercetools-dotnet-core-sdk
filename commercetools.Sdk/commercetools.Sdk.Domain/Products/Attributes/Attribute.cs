namespace commercetools.Sdk.Domain.Products.Attributes
{
    public abstract class Attribute
    {
        public string Name { get; set; }

        public EnumAttribute ToEnumAttribute()
        {
            return this as EnumAttribute;
        }

        public LocalizedTextAttribute ToLocalizedTextAttribute()
        {
            return this as LocalizedTextAttribute;
        }

        public TextAttribute ToTextAttribute()
        {
            return this as TextAttribute;
        }
    }
}