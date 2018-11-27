using System.Collections.Generic;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public class NestedLambdaExpressionVisitor : Visitor
    {
        private string left;
        private Visitor innerResult;
        private List<string> parentList = new List<string>();

        public NestedLambdaExpressionVisitor(MethodCallExpression expression)
        {
            var body = ((LambdaExpression)expression.Arguments[1]).Body;
            // TODO Try to change to static
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            innerResult = queryPredicateExpressionVisitor.VisitExpression(body);
            parentList = QueryPredicateExpressionVisitor.GetParentMemberList(expression.Arguments[0]);
            if (expression.Arguments[0].NodeType == ExpressionType.MemberAccess)
            {
                left = ((MemberExpression)expression.Arguments[0]).Member.Name;
            }
        }

        public override string ToString()
        {
            string result = $"{left.ToCamelCase()}({innerResult.ToString()})";
            return QueryPredicateExpressionVisitor.Visit(result, parentList);
        }
    }
}