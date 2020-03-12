namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetMoneyAttribute : SetAttribute<Money>
    {
        public SetMoneyAttribute()
        {
        }
        public SetMoneyAttribute(SetAttribute<Money> attribute)
        {
            CloneProperties(attribute);
        }
    }
}