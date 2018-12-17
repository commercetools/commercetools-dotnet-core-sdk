namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public class BinaryLogicalPredicateVisitor : IPredicateVisitor
    {
        private readonly IPredicateVisitor left;
        private readonly IPredicateVisitor right;

        private readonly string operatorSign;

        public BinaryLogicalPredicateVisitor(IPredicateVisitor left, string operatorSign, IPredicateVisitor right)
        {
            this.left = left;
            this.operatorSign = operatorSign;
            this.right = right;
        }

        public string Render()
        {
            // If one of the sides has an operator with lower precedence, it has to be wrapped in brackets.
            return $"{this.RenderSide(this.left)} {this.operatorSign} {this.RenderSide(this.right)}";
        }

        private string RenderSide(IPredicateVisitor side)
        {
            string result = side.Render();
            if (this.HasLowerPrecedenceOperator(side))
            {
                result = $"({result})";
            }

            return result;
        }

        private bool HasLowerPrecedenceOperator(IPredicateVisitor side)
        {
            if (side is BinaryLogicalPredicateVisitor logicalPredicateVisitor)
            {
                if (logicalPredicateVisitor.operatorSign == Mapping.Or && this.operatorSign == Mapping.And)
                {
                    return true;
                }
            }

            return false;
        }
    }
}