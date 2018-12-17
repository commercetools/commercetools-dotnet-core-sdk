namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public class ComparisonPredicateVisitor : IPredicateVisitor, IAccessorAppendable
    {
        private readonly string operatorSign;
        private readonly IPredicateVisitor right;
        private IPredicateVisitor left;

        public ComparisonPredicateVisitor(IPredicateVisitor left, string operatorSign, IPredicateVisitor right)
        {
            this.left = left;
            this.right = right;
            this.operatorSign = operatorSign;
        }

        public void AppendAccessor(AccessorPredicateVisitor accessor)
        {
            if (this.left != null)
            {
                if (this.left is IAccessorAppendable accessorAppendablePredicate)
                {
                    accessorAppendablePredicate.AppendAccessor(accessor);
                }
            }
            else
            {
                this.left = accessor;
            }
        }

        public string Render()
        {
            return $"{this.left?.Render()} {this.operatorSign} {this.right?.Render()}";
        }
    }
}