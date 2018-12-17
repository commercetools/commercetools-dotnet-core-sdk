namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public class UnaryPredicateVisitor : IPredicateVisitor, IAccessorAppendable
    {
        private readonly string operatorName;
        private IPredicateVisitor operand;

        public UnaryPredicateVisitor(IPredicateVisitor operand, string operatorName)
        {
            this.operand = operand;
            this.operatorName = operatorName;
        }

        public void AppendAccessor(AccessorPredicateVisitor accessor)
        {
            if (this.operand != null)
            {
                if (this.operand is IAccessorAppendable accessorAppendablePredicate)
                {
                    accessorAppendablePredicate.AppendAccessor(accessor);
                }
            }
            else
            {
                this.operand = accessor;
            }
        }

        public string Render()
        {
            return $"{this.operand?.Render()} {this.operatorName}";
        }
    }
}