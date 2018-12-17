using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class MethodPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (Mapping.AllowedMethods.ContainsKey(methodCallExpression.Method.Name))
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

            if (methodCallExpression.Arguments.Count == 1)
            {
                string operatorName = Mapping.AllowedMethods[methodCallExpression.Method.Name];
                IPredicateVisitor cartPredicateVisitor = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                return new UnaryPredicateVisitor(cartPredicateVisitor, operatorName);
            }

            if (methodCallExpression.Arguments.Count == 2)
            {
                string operatorName = Mapping.AllowedMethods[methodCallExpression.Method.Name];
                IPredicateVisitor cartPredicateVisitor = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                List<string> arguments = GetArguments(methodCallExpression.Arguments[1]);
                CollectionPredicateVisitor collectionPredicateVisitor = new CollectionPredicateVisitor(arguments);
                return new ComparisonPredicateVisitor(cartPredicateVisitor, operatorName, collectionPredicateVisitor);
            }

            return null;
        }

        private static List<string> GetArguments(Expression expression)
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
    }
}