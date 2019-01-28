using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class WithinCirclePredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public Dictionary<string, string> AllowedMethods { get; } = new Dictionary<string, string>()
        {
            { "WithinCircle", "within circle" }
        };

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (this.AllowedMethods.ContainsKey(methodCallExpression.Method.Name))
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

            string methodName = this.AllowedMethods[methodCallExpression.Method.Name];
            ConstantPredicateVisitor methodNameConstant = new ConstantPredicateVisitor(methodName);
            IPredicateVisitor methodCaller = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            IPredicateVisitor methodArguments = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);

            ContainerPredicateVisitor container = new ContainerPredicateVisitor(methodArguments, methodNameConstant, true);
            BinaryPredicateVisitor binary = new BinaryPredicateVisitor(methodCaller, string.Empty, container);
            return binary;
        }
    }
}
