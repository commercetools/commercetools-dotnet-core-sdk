namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class MoneyAttribute : Attribute<Money>
    {
        public MoneyAttribute()
        {
        }
        public MoneyAttribute(Attribute<Money> attribute)
        {
            CloneProperties(attribute);
        }
    }
}