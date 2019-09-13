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
                    IPredicateVisitor attributeName = GetAttributeName(((BinaryExpression)attributeExpression).Left, predicateVisitorFactory);
                    AccessorPredicateVisitor parentAccessor = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]) as AccessorPredicateVisitor;
                    AccessorPredicateVisitor accessor = new AccessorPredicateVisitor(attributeName, parentAccessor);
                    if (attributeValuePredicateVisitor is IAccessorAppendable accessorAppendablePredicate)
                    {
                        accessorAppendablePredicate.AppendAccessor(accessor);
                    }

                    return attributeValuePredicateVisitor;
                }
            }

            throw new NotSupportedException();
        }

        private static IPredicateVisitor GetAttributeName(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            if (expression is BinaryExpression nameExpression)
            {
                if (nameExpression.Left is MemberExpression memberExpression && memberExpression.Member.Name == "Name")
                {
                    return RemoveQuotesWithCaseSensitive(predicateVisitorFactory.Create(nameExpression.Right));
                }
            }

            return null;
        }

        /// <summary>
        /// Remove Quotes and make it constant predicate visitor with case sensitive
        /// Attribute names must be case sensitive
        /// </summary>
        /// <param name="inner">The inner Constant Predicate Visitor</param>
        /// <returns>Constant Predicate Visitor</returns>
        private static IPredicateVisitor RemoveQuotesWithCaseSensitive(IPredicateVisitor inner)
        {
            if (inner is ConstantPredicateVisitor constantVisitor)
            {
                ConstantPredicateVisitor constantWithoutQuotes = new ConstantPredicateVisitor(constantVisitor.Constant.RemoveQuotes().WrapInBackticksIfNeeded(), true);
                return constantWithoutQuotes;
            }

            return inner;
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
