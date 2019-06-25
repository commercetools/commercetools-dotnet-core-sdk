using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    /// <summary>
    /// For comparison of money like (c.TotalPrice == money.moneyString())
    /// </summary>
    public class MoneyStringPredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        private const string MethodName = "moneyString";

        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression && methodCallExpression.Method.Name == MethodName)
            {
                return true;
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            var dynamicInvoke = Expression.Lambda(expression, null).Compile().DynamicInvoke(null);
            var compiledValue = dynamicInvoke.ToString().WrapInQuotes();
            return new ConstantPredicateVisitor(compiledValue);
        }
    }
}
