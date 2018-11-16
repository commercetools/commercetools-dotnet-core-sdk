using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace commercetools.Sdk.Linq
{
    public class MethodCallExpressionVisitor : BinaryExpressionVisitor
    {
        private Dictionary<string, string> mappingOfMethods = new Dictionary<string, string>()
        {
            { "In", "in" },
            { "NotIn", "not in" },
            { "ContainsAll", "contains all" }
        };

        public MethodCallExpressionVisitor(MethodCallExpression expression)
        {
            operatorSign = this.mappingOfMethods[expression.Method.Name];
            parentList = QueryPredicateExpressionVisitor.GetParentMemberList(expression.Arguments[0]);
            if (expression.Arguments[0].NodeType == ExpressionType.MemberAccess)
            {
                left = ((MemberExpression)expression.Arguments[0]).Member.Name;
            }

            List<string> rightList = new List<string>();
            if (expression.Arguments[1].NodeType == ExpressionType.NewArrayInit)
            {
                foreach (Expression part in ((NewArrayExpression)expression.Arguments[1]).Expressions)
                {
                    if (part.NodeType == ExpressionType.Constant)
                    {
                        rightList.Add(part.ToString());
                    }
                }
            }

            if (rightList.Count() > 0)
            {
                right = $"({string.Join(", ", rightList)})";
            }
        }
    }
}
