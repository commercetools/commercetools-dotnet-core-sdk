namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetTextAttribute : SetAttribute<string>
    {
        public SetTextAttribute()
        {
        }
        public SetTextAttribute(SetAttribute<string> attribute)
        {
            CloneProperties(attribute);
        }
    }
}