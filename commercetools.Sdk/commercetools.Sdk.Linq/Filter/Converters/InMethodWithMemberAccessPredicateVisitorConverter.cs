using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class InMethodWithMemberAccessPredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "In" && methodCallExpression.Arguments[1].NodeType == ExpressionType.MemberAccess)
                {
                    return true;
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return null;
            }

            IPredicateVisitor parent = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            List<IPredicateVisitor> parameters = new List<IPredicateVisitor>();

            var memberExpression =
                (MemberExpression)methodCallExpression.Arguments[1];

            if (Expression.Lambda(memberExpression).Compile().DynamicInvoke() is IEnumerable<object> arr)
            {
                foreach (var element in arr)
                {
                    parameters.Add(
                        new ConstantPredicateVisitor(element.ToString().WrapInQuotes()));
                }
            }

            CollectionPredicateVisitor collection = new CollectionPredicateVisitor(parameters);
            EqualPredicateVisitor equal = new EqualPredicateVisitor(parent, collection);
            return equal;
        }
    }
}
