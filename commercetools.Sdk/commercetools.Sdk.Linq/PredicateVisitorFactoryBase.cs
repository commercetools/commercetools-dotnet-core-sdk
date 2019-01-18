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
                // c.Key == "c14" in c => c.Key == "c14"
                case ExpressionType.Lambda:
                    return this.Create(((LambdaExpression)expression).Body);

                // LineItemCount(this Cart source, Expression<Func<LineItem, bool>> parameter)
                // Happens when an expression is passed as a method parameter.
                case ExpressionType.Quote:
                    return this.Create(((UnaryExpression)expression).Operand);

                // c in c.Key
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