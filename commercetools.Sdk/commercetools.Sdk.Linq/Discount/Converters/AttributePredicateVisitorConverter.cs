using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class AttributePredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (IsMethodNameAllowed(methodCallExpression) && IsValidMethodCaller(methodCallExpression))
                {
                    return true;
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (expression == null)
            {
                return null;
            }

            var innerExpression = methodCallExpression?.Arguments[1];
            if (innerExpression is LambdaExpression lambdaExpression)
            {
                var attributeExpression = lambdaExpression.Body;
                if (attributeExpression.NodeType == ExpressionType.And || attributeExpression.NodeType == ExpressionType.AndAlso)
                {
                    IPredicateVisitor attributeValuePredicateVisitor = predicateVisitorFactory.Create(((BinaryExpression)attributeExpression).Right);
                    string attributeName = GetAttributeName(((BinaryExpression)attributeExpression).Left);
                    AccessorPredicateVisitor parentAccessor = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]) as AccessorPredicateVisitor;
                    ConstantPredicateVisitor constantPredicateVisitor = new ConstantPredicateVisitor(attributeName);
                    AccessorPredicateVisitor accessor = new AccessorPredicateVisitor(constantPredicateVisitor, parentAccessor);
                    if (attributeValuePredicateVisitor is IAccessorAppendable accessorAppendablePredicate)
                    {
                        accessorAppendablePredicate.AppendAccessor(accessor);
                    }

                    return attributeValuePredicateVisitor;
                }
            }

            throw new NotSupportedException();
        }

        private static string GetAttributeName(Expression expression)
        {
            if (expression is BinaryExpression nameExpression)
            {
                if (nameExpression.Left is MemberExpression memberExpression && memberExpression.Member.Name == "Name")
                {
                    return nameExpression.Right.ToString().Replace("\"", "");
                }
            }
            throw new NotSupportedException();
        }

        private static bool IsMethodNameAllowed(MethodCallExpression expression)
        {
            return expression.Method.Name == "Any";
        }

        private static bool IsValidMethodCaller(MethodCallExpression expression)
        {
            Expression callerExpression = expression.Arguments[0];
            if (callerExpression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "Attributes")
                {
                    return true;
                }
            }

            return false;
        }
    }
}