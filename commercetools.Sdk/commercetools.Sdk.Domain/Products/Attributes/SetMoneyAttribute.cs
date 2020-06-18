namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetMoneyAttribute : SetAttribute<Money>
    {
        public SetMoneyAttribute()
        {
        }
        public SetMoneyAttribute(Attribute<AttributeSet<Money>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}