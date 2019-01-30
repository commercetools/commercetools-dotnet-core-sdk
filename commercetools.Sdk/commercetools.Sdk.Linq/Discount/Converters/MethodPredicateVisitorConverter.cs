using System;
using System.Collections.Generic;
using System.Linq;
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
                IEnumerable<IPredicateVisitor> arguments = GetArguments(methodCallExpression.Arguments[1], predicateVisitorFactory);
                if (arguments.ToList().Count > 1)
                {
                    CollectionPredicateVisitor collectionPredicateVisitor = new CollectionPredicateVisitor(arguments);
                    return new ComparisonPredicateVisitor(cartPredicateVisitor, operatorName, collectionPredicateVisitor);
                }
                else
                {
                    return new ComparisonPredicateVisitor(cartPredicateVisitor, operatorName, arguments.First());
                }
            }

            return null;
        }

        private static IEnumerable<IPredicateVisitor> GetArguments(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            List<IPredicateVisitor> arguments = new List<IPredicateVisitor>();
            if (expression.NodeType == ExpressionType.NewArrayInit)
            {
                foreach (Expression part in ((NewArrayExpression)expression).Expressions)
                {
                    IPredicateVisitor predicateVisitor = predicateVisitorFactory.Create(part);
                    arguments.Add(predicateVisitor);
                }
            }
            else
            {
                IPredicateVisitor predicateVisitor = predicateVisitorFactory.Create(expression);
                arguments.Add(predicateVisitor);
            }

            return arguments;
        }
    }
}