using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public class CartPredicateVisitorFactory : ICartPredicateVisitorFactory
    {
        private readonly IEnumerable<ICartPredicateVisitorConverter> registeredConverters;

        public CartPredicateVisitorFactory(IEnumerable<ICartPredicateVisitorConverter> registeredConverters)
        {
            this.registeredConverters = registeredConverters;
        }

        public ICartPredicateVisitor Create(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
            {
                return this.Create(((LambdaExpression)expression).Body);
            }
            if (expression.NodeType == ExpressionType.Quote)
            {
                return this.Create(((UnaryExpression)expression).Operand);
            }
            if (expression.NodeType == ExpressionType.Parameter)
            {
                return null;
            }
            var converter = this.GetConverterForExpression(expression);
            return converter.Convert(expression, this);
        }

        private ICartPredicateVisitorConverter GetConverterForExpression(Expression expression)
        {
            foreach (var converter in this.registeredConverters)
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