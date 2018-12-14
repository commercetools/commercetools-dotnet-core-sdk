namespace commercetools.Sdk.Linq.Carts
{
    public class BinaryLogicalPredicateVisitor : ICartPredicateVisitor
    {
        private readonly ICartPredicateVisitor left;
        private readonly ICartPredicateVisitor right;

        private readonly string operatorSign;

        public BinaryLogicalPredicateVisitor(ICartPredicateVisitor left, string operatorSign, ICartPredicateVisitor right)
        {
            this.left = left;
            this.operatorSign = operatorSign;
            this.right = right;
        }

        public string Render()
        {
            // If one of the side has an operator with lower precedence, it has to be wrapped in brackets.
            return $"{this.RenderSide(this.left)} {this.operatorSign} {this.RenderSide(this.right)}";
        }

        private string RenderSide(ICartPredicateVisitor side)
        {
            string result = side.Render();
            if (this.HasLowerPrecedenceOperator(side))
            {
                result = $"({result})";
            }

            return result;
        }

        private bool HasLowerPrecedenceOperator(ICartPredicateVisitor side)
        {
            if (side is BinaryLogicalPredicateVisitor logicalPredicateVisitor)
            {
                if (logicalPredicateVisitor.operatorSign == LogicalOperators.OR && this.operatorSign == LogicalOperators.AND)
                {
                    return true;
                }
            }

            return false;
        }
    }
}