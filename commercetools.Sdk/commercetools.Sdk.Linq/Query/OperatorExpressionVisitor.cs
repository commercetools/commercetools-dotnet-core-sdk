using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class OperatorExpressionVisitor : BinaryExpressionVisitor
    {
        public OperatorExpressionVisitor(BinaryExpression expression)
        {
            if (expression.Left.NodeType == ExpressionType.MemberAccess)
            {
                left = ((MemberExpression)expression.Left).Member.Name;
                this.parentList = QueryPredicateExpressionVisitor.GetParentMemberList(expression.Left);
            }
            if (expression.Left.NodeType == ExpressionType.Call)
            {
                // TODO See if a check should be made to see if this is a dictionary<string, string>
                object name = ((MethodCallExpression)expression.Left).Object;
                if (((MethodCallExpression)expression.Left).Object.NodeType == ExpressionType.MemberAccess)
                {
                    this.parentList.Add(((MemberExpression)((MethodCallExpression)expression.Left).Object).Member.Name);
                    this.parentList.AddRange(QueryPredicateExpressionVisitor.GetParentMemberList(((MethodCallExpression)expression.Left).Object));
                }
                if (((MethodCallExpression)expression.Left).Arguments[0].NodeType == ExpressionType.Constant)
                {
                    this.left = ((MethodCallExpression)expression.Left).Arguments[0].ToString().Replace("\"", "");
                }
            }
            if (expression.Right.NodeType == ExpressionType.Constant)
            {
                this.right = expression.Right.ToString();
            }

            this.operatorSign = QueryPredicateExpressionVisitor.MappingOfOperators[expression.NodeType];
        }
    }
}
