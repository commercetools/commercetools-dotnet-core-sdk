using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class CustomFieldsConverter : ICartPredicateVisitorConverter
    {
        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "get_Item")
                {
                    if (methodCallExpression.Object is MemberExpression memberExpression)
                    { 
                        if (memberExpression.Member.Name == "Fields")
                        { 
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                var key = methodCallExpression.Arguments[0].ToString().Replace("\"", "");

                if (methodCallExpression.Object is MemberExpression memberExpression)
                {
                    ICartPredicateVisitor parentAccessors = cartPredicateVisitorFactory.Create(memberExpression.Expression);
                    return new Accessor(key, parentAccessors);
                }
            }
            throw new NotSupportedException();
        }
    }
}
