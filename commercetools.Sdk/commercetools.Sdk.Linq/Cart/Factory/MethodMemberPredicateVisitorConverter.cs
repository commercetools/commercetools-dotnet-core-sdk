using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Carts;

namespace commercetools.Sdk.Linq
{
    public class MethodMemberPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (Mappings.MethodAccessors.ContainsKey(methodCallExpression.Method.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                string currentName = methodCallExpression.Method.Name;
                string currentAccessor = ParseMethodAccessorName(currentName);
                if (string.IsNullOrEmpty(currentAccessor))
                {
                    return cartPredicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                }
                else
                {
                    Accessor parentAccessor = cartPredicateVisitorFactory.Create(methodCallExpression.Arguments[0]) as Accessor;
                    ConstantPredicateVisitor constantPredicateVisitor = new ConstantPredicateVisitor(currentAccessor);
                    return new Accessor(constantPredicateVisitor, parentAccessor);
                }
            }
            throw new NotSupportedException();
        }

        private string ParseMethodAccessorName(string name)
        {
            if (Mappings.MethodAccessors.ContainsKey(name))
            {
                return Mappings.MethodAccessors[name];
            }
            return name;
        }
    }
}