namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class MoneyAttribute : Attribute<BaseMoney>
    {
        public MoneyAttribute()
        {
        }
        public MoneyAttribute(Attribute<BaseMoney> attribute)
        {
            CloneProperties(attribute);
        }
    }
}