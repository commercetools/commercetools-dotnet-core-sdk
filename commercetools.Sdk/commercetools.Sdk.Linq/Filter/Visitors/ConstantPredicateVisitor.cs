namespace commercetools.Sdk.Linq.Filter.Visitors
{
    public class ConstantPredicateVisitor : IPredicateVisitor
    {
        public ConstantPredicateVisitor(string constant)
        {
            this.Constant = constant;
        }

        public string Constant { get; }

        public string Render()
        {
            return $"{this.Constant.ToCamelCase()}";
        }
    }
}
