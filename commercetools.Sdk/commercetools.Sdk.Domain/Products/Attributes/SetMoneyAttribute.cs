namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class SetMoneyAttribute : SetAttribute<BaseMoney>
    {
        public SetMoneyAttribute()
        {
        }
        public SetMoneyAttribute(Attribute<AttributeSet<BaseMoney>> attribute)
        {
            CloneProperties(attribute);
        }
    }
}