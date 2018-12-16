using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class AttributePredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            return IsAttributeWithValue(expression) || IsAttributeWithNameOnly(expression);
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression == null)
            {
                return null;
            }

            // This is attribute with name only.
            if (binaryExpression.NodeType == ExpressionType.Equal)
            {
                return GetName(binaryExpression, predicateVisitorFactory);
            }

            IPredicateVisitor attributeName = GetName(binaryExpression.Left, predicateVisitorFactory);
            IPredicateVisitor attributeValue = GetValue(binaryExpression.Right, predicateVisitorFactory);
            return new AccessorPredicateVisitor(attributeValue, attributeName);
        }

        private static IPredicateVisitor GetName(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression == null)
            {
                return null;
            }

            if (binaryExpression.Left is MemberExpression memberExpression && memberExpression.Member.Name == "Name")
            {
                IPredicateVisitor name = predicateVisitorFactory.Create(binaryExpression.Right);
                return RemoveQuotes(name);
            }

            return null;
        }

        // TODO Combine this with other similar methods.
        private static IPredicateVisitor RemoveQuotes(IPredicateVisitor inner)
        {
            if (inner is ConstantPredicateVisitor constantVisitor)
            {
                ConstantPredicateVisitor constantWithoutQuotes = new ConstantPredicateVisitor(constantVisitor.Constant.RemoveQuotes());
                return constantWithoutQuotes;
            }

            return inner;
        }

        private static IPredicateVisitor GetValue(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            return predicateVisitorFactory.Create(expression);
        }

        private static bool IsAttributeWithValue(Expression expression)
        {
            // Right now this is only used for attributes.
            // In case this has to be used for something else, a stricter check needs to be performed.
            return expression.NodeType == ExpressionType.And || expression.NodeType == ExpressionType.AndAlso;
        }

        private static bool IsAttributeWithNameOnly(Expression expression)
        {
            if (expression.NodeType != ExpressionType.Equal)
            {
                return false;
            }

            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression?.Left.NodeType != ExpressionType.MemberAccess)
            {
                return false;
            }

            MemberExpression member = binaryExpression?.Left as MemberExpression;
            if (member?.Member.Name == "Name")
            {
                return true;
            }

            return false;
        }
    }
}
