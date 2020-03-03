using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    // "c14", "c15" in c.Key.In("c14", "c15")
    public class CollectionPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.NewArrayInit || IsArray(expression);
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            List<IPredicateVisitor> predicateVisitors = new List<IPredicateVisitor>();
            NewArrayExpression arrayExpression = expression as NewArrayExpression;
            if (arrayExpression != null)
            {
                foreach (var innerExpression in arrayExpression.Expressions)
                {
                    IPredicateVisitor innerPredicateVisitor = predicateVisitorFactory.Create(innerExpression);
                    predicateVisitors.Add(innerPredicateVisitor);
                }
            }
            else if (IsArray(expression))
            {
                var dynamicInvoke = Expression.Lambda(expression, null).Compile().DynamicInvoke(null);
                if (dynamicInvoke is Array array)
                {
                    foreach (var item in array)
                    {
                        var constant = new ConstantPredicateVisitor(item.ToString().WrapInQuotes());
                        predicateVisitors.Add(constant);
                    }
                }
            }

            return new CollectionPredicateVisitor(predicateVisitors);
        }

        private static bool IsArray(Expression expression)
        {
            if (expression.NodeType != ExpressionType.MemberAccess)
            {
                return false;
            }

            MemberExpression memberExpression = expression as MemberExpression;

            if (memberExpression?.Member is FieldInfo fieldInfo)
            {
                return fieldInfo.FieldType.IsArray;
            }

            return false;
        }
    }
}
