using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class LogicalPredicateVisitor : ICartPredicateVisitor
    {
        private readonly ICartPredicateVisitor left;
        private readonly ICartPredicateVisitor right;

        public string OperatorSign { get; }

        public LogicalPredicateVisitor(ICartPredicateVisitor left, string operatorSign, ICartPredicateVisitor right)
        {
            this.left = left;
            this.OperatorSign = operatorSign;
            this.right = right;
        }

        public string Render()
        {
            // If one of the side has an operator with lower precedence, it has to be wrapped in brackets
            return $"{RenderSide(this.left)} {this.OperatorSign} {RenderSide(this.right)}";
        }

        private string RenderSide(ICartPredicateVisitor side)
        {
            string result = side.Render();
            if (HasLowerPrecedenceOperator(side, this.OperatorSign))
            {
                result = $"({result})";
            }
            return result;
        }

        private bool HasLowerPrecedenceOperator(ICartPredicateVisitor side, string operatorSign)
        {
            if (side is LogicalPredicateVisitor logicalPredicateVisitor)
            {
                if (logicalPredicateVisitor.OperatorSign == LogicalOperators.OR && operatorSign == LogicalOperators.AND)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
