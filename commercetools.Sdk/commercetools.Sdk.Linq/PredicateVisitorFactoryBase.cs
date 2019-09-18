using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Linq
{
    public abstract class PredicateVisitorFactoryBase : IPredicateVisitorFactory
    {
        private readonly IEnumerable<IPredicateVisitorConverter> registeredConverters;

        protected PredicateVisitorFactoryBase(IEnumerable<IPredicateVisitorConverter> registeredConverters)
        {
            this.registeredConverters = registeredConverters;
        }

        public static IEnumerable<IPredicateVisitorConverter> GetPredicateVisitorConverters<T>()
            where T : IPredicateVisitorConverter
        {
            var retriever = new TypeRetriever();
            var types = retriever.GetTypes<T>();

            return types.Select(type => (IPredicateVisitorConverter)Activator.CreateInstance(type));
        }

        public IPredicateVisitor Create(Expression expression)
        {
            switch (expression.NodeType)
            {
                // c.Key == "c14" in c => c.Key == "c14"
                case ExpressionType.Lambda:
                    var lambdaExpression = (LambdaExpression) expression;
                    if (lambdaExpression.ReturnType == typeof(bool))
                    {
                        if (lambdaExpression.Body is MemberExpression memberExpression)
                        {
                            // Special case: c => c.Key
                            return this.Create(Expression.Equal(memberExpression, Expression.Constant(true)));
                        }

                        if (lambdaExpression.Body is UnaryExpression unaryExpression &&
                            unaryExpression.NodeType == ExpressionType.Not)
                        {
                            // Special case: c => !c.Key
                            return this.Create(Expression.Equal(unaryExpression.Operand, Expression.Constant(false)));
                        }
                    }

                    return this.Create(lambdaExpression.Body);

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
