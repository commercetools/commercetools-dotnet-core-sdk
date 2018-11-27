namespace commercetools.Sdk.Linq
{
    public class UnaryPredicateVisitor : ICartPredicateVisitor, IAccessorAppendable
    {
        private ICartPredicateVisitor operand;
        private string operatorName;

        public UnaryPredicateVisitor(ICartPredicateVisitor operand, string operatorName)
        {
            this.operand = operand;
            this.operatorName = operatorName;
        }

        public void AppendAccessor(Accessor accessor)
        {
            if (operand != null)
            {
                if (this.operand is IAccessorAppendable accessorAppendablePredicate)
                {
                    accessorAppendablePredicate.AppendAccessor(accessor);
                }
            }
            else
            {
                operand = accessor;
            }
        }

        public string Render()
        {
            return $"{this.operand?.Render()} {operatorName}";
        }
    }
}