using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class GroupExpressionFlattener
    {
        // TODO this is for now doubled, see how to declare this in one place only
        private static List<ExpressionType> allowedGroupingExpressionTypes = new List<ExpressionType>() { ExpressionType.Or, ExpressionType.OrElse, ExpressionType.And, ExpressionType.AndAlso };

        public static List<FilterVisitor> GetFilterVisitors(BinaryExpression expression)
        {
            return FlattenOrFilters(expression);
        }

        private static List<FilterVisitor> FlattenOrFilters(BinaryExpression expression)
        {
            List<FilterVisitor> filterValues = new List<FilterVisitor>();
            Expression left = expression.Left;
            Expression right = expression.Right;
            if (allowedGroupingExpressionTypes.Contains(left.NodeType))
            {
                filterValues.AddRange(FlattenOrFilters((BinaryExpression)left));
            }
            else
            {
                filterValues.Add(GetFilterValue(left));
            }
            if (allowedGroupingExpressionTypes.Contains(right.NodeType))
            {
                filterValues.AddRange(FlattenOrFilters((BinaryExpression)right));
            }
            else
            {
                filterValues.Add(GetFilterValue(right));
            }
            return filterValues;
        }

        private static FilterVisitor GetFilterValue(Expression expression)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                return new EqualFilterVisitor(binaryExpression);
            }
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "Range")
                {
                    return new RangeFilterVisitor(methodCallExpression);
                }
                if (methodCallExpression.Method.Name == "Subtree")
                {
                    return new SubtreeFilterVisitor(methodCallExpression);
                }
                // TODO Move message to a resource file
                throw new NotSupportedException("The expression type is not supported.");
            }
            // TODO Move message to a resource file
            throw new NotSupportedException("The expression type is not supported.");
        }
    }
}
