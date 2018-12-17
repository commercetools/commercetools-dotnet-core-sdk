namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public class ConstantPredicateVisitor : IPredicateVisitor
    {
        private readonly string constant;

        public ConstantPredicateVisitor(string constant)
        {
            this.constant = constant;
        }

        public string Render()
        {
            return this.constant.ToCamelCase();
        }
    }
}