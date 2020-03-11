namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetNumberAttribute : SetAttribute<double>
    {
        public SetNumberAttribute()
        {
        }
        public SetNumberAttribute(SetAttribute<double> attribute)
        {
            CloneProperties(attribute);
        }
    }
}