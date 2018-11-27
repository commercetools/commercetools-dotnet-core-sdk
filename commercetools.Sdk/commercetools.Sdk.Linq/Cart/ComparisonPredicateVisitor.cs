namespace commercetools.Sdk.Linq
{
    public class ComparisonPredicateVisitor : ICartPredicateVisitor, IAccessorAppendable
    {
        private ICartPredicateVisitor left;
        private ICartPredicateVisitor right;
        private string operatorSign;

        public ComparisonPredicateVisitor(ICartPredicateVisitor left, string operatorSign, ICartPredicateVisitor right)
        {
            this.left = left;
            this.right = right;
            this.operatorSign = operatorSign;
        }

        public void AppendAccessor(Accessor accessor)
        {
            if (left != null)
            {
                if (this.left is IAccessorAppendable accessorAppendablePredicate)
                {
                    accessorAppendablePredicate.AppendAccessor(accessor);
                }
            }
            else
            {
                left = accessor;
            }
        }

        public string Render()
        {
            return $"{left?.Render()} {operatorSign} {right?.Render()}";
        }
    }
}