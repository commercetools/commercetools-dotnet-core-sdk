using System;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class MemberPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.MemberAccess;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            if (expression is MemberExpression memberExpression)
            {
                string currentName = memberExpression.Member.Name;
                string currentAccessor = ParseName(ParseAccessorName(currentName));
                if (string.IsNullOrEmpty(currentAccessor))
                {
                    return predicateVisitorFactory.Create(memberExpression.Expression);
                }

                AccessorPredicateVisitor parentAccessor = predicateVisitorFactory.Create(memberExpression.Expression) as AccessorPredicateVisitor;
                ConstantPredicateVisitor constantPredicateVisitor = new ConstantPredicateVisitor(currentAccessor);
                return new AccessorPredicateVisitor(constantPredicateVisitor, parentAccessor);
            }

            throw new NotSupportedException();
        }

        private static string ParseAccessorName(string name)
        {
            if (DiscountMapping.PropertyAccessors.ContainsKey(name))
            {
                return DiscountMapping.PropertyAccessors[name];
            }

            return name;
        }

        private static string ParseName(string currentName)
        {
            if (!DiscountMapping.AccessorsToSkip.Contains(currentName))
            {
                return currentName;
            }

            return null;
        }
    }
}