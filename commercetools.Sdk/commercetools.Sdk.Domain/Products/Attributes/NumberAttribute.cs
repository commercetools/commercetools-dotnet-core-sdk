namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class NumberAttribute : Attribute<double>
    {
        public NumberAttribute()
        {
        }
        public NumberAttribute(Attribute<double> attribute)
        {
            CloneProperties(attribute);
        }
    }
}
