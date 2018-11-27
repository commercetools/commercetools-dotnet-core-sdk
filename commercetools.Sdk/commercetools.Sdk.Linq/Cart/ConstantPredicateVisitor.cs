namespace commercetools.Sdk.Linq
{
    public class ConstantPredicateVisitor : ICartPredicateVisitor
    {
        private string constant;

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