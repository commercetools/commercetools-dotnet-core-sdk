using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public class MethodPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        private readonly Dictionary<string, string> allowedMethods = new Dictionary<string, string>()
        {
            { "ContainsAny", "contains any" },
            { "ContainsAll", "contains all" },
            { "Contains", "contains" },
            { "IsEmpty", "is empty" },
            { "IsNotEmpty", "is not empty" },
            { "IsDefined", "is defined" },
            { "IsNotDefined", "is not defined" }
        };

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (this.allowedMethods.ContainsKey(methodCallExpression.Method.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new ArgumentException();
            }
            if (methodCallExpression.Arguments.Count == 1)
            {
                string operatorName = GetMethodName(methodCallExpression);
                ICartPredicateVisitor cartPredicateVisitor = cartPredicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                return new UnaryPredicateVisitor(cartPredicateVisitor, operatorName);
            }
            if (methodCallExpression.Arguments.Count == 2)
            {
                string operatorName = this.GetMethodName(methodCallExpression);
                ICartPredicateVisitor cartPredicateVisitor = cartPredicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                List<string> arguments = this.GetArguments(methodCallExpression.Arguments[1]);
                CollectionPredicateVisitor collectionPredicateVisitor = new CollectionPredicateVisitor(arguments);
                return new ComparisonPredicateVisitor(cartPredicateVisitor, operatorName, collectionPredicateVisitor);
            }
            throw new NotSupportedException();
        }

        private List<string> GetArguments(Expression expression)
        {
            List<string> arguments = new List<string>();
            if (expression.NodeType == ExpressionType.NewArrayInit)
            {
                foreach (Expression part in ((NewArrayExpression)expression).Expressions)
                {
                    if (part.NodeType == ExpressionType.Constant)
                    {
                        arguments.Add(part.ToString());
                    }
                }
            }
            return arguments;
        }

        private string GetMethodName(MethodCallExpression expression)
        {
            if (this.allowedMethods.ContainsKey(expression.Method.Name))
            {
                return this.allowedMethods[expression.Method.Name];
            }
            throw new NotSupportedException();
        }
    }
}