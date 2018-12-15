using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public abstract class PredicateVisitorFactoryBase : IPredicateVisitorFactory
    {
        private readonly IEnumerable<IPredicateVisitorConverter> registeredConverters;

        protected PredicateVisitorFactoryBase(IEnumerable<IPredicateVisitorConverter> registeredConverters)
        {
            this.registeredConverters = registeredConverters;
        }

        public IPredicateVisitor Create(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return this.Create(((LambdaExpression)expression).Body);
                case ExpressionType.Quote:
                    return this.Create(((UnaryExpression)expression).Operand);
                case ExpressionType.Parameter:
                    return null;
                default:
                {
                    var converter = this.GetConverterForExpression(expression);
                    return converter.Convert(expression, this);
                }
            }
        }

        private IPredicateVisitorConverter GetConverterForExpression(Expression expression)
        {
            foreach (var converter in this.registeredConverters.OrderBy(c => c.Priority))
            {
                if (converter.CanConvert(expression))
                {
                    return converter;
                }
            }

            throw new NotSupportedException();
        }
    }
}