//using System.Linq.Expressions;

//namespace commercetools.Sdk.Linq
//{
//    public class OperatorExpressionVisitor : BinaryExpressionVisitor
//    {
//        public OperatorExpressionVisitor(BinaryExpression expression)
//        {
//            if (expression.Left.NodeType == ExpressionType.MemberAccess)
//            {
//                left = ((MemberExpression)expression.Left).Member.Name;
//                this.parentList = QueryPredicateExpressionVisitor.GetParentMemberList(expression.Left);
//            }
//            if (expression.Left.NodeType == ExpressionType.Call)
//            {
//                // TODO See if a check should be made to see if this is a dictionary<string, string>
//                object name = ((MethodCallExpression)expression.Left).Object;
//                if (((MethodCallExpression)expression.Left).Object.NodeType == ExpressionType.MemberAccess)
//                {
//                    this.parentList.Add(((MemberExpression)((MethodCallExpression)expression.Left).Object).Member.Name);
//                    this.parentList.AddRange(QueryPredicateExpressionVisitor.GetParentMemberList(((MethodCallExpression)expression.Left).Object));
//                }
//                if (((MethodCallExpression)expression.Left).Arguments[0].NodeType == ExpressionType.Constant)
//                {
//                    this.left = ((MethodCallExpression)expression.Left).Arguments[0].ToString().Replace("\"", "");
//                }
//            }
//            if (expression.Right.NodeType == ExpressionType.Constant)
//            {
//                this.right = expression.Right.ToString();
//            }
//            if (expression.Right.NodeType == ExpressionType.MemberAccess)
//            {
//                MemberExpression memberExpression = expression.Right as MemberExpression;
//                var x = Expression.Lambda(expression.Right, null).Compile().DynamicInvoke(null);
//                if (memberExpression.Type == typeof(string))
//                {
//                    x = string.Format("\"{0}\"", x);
//                }
//                this.right = x.ToString();
//            }

//            this.operatorSign = QueryPredicateExpressionVisitor.MappingOfOperators[expression.NodeType];
//        }
//    }
//}