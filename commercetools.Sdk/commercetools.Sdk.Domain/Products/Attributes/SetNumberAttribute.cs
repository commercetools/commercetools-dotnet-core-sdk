namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetNumberAttribute : SetAttribute<double>
    {
        public SetNumberAttribute()
        {
        }
        public SetNumberAttribute(Attribute<AttributeSet<double>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}
