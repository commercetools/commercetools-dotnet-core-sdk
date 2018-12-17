using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class DictionaryPredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            return IsDictionary(expression);
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return null;
            }

            IPredicateVisitor inner = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            IPredicateVisitor innerWithoutQuotes = RemoveQuotes(inner);
            IPredicateVisitor parent = predicateVisitorFactory.Create(methodCallExpression.Object);
            return new Visitors.AccessorPredicateVisitor(innerWithoutQuotes, parent);
        }

        private static IPredicateVisitor RemoveQuotes(IPredicateVisitor inner)
        {
            if (inner is ConstantPredicateVisitor constantVisitor)
            {
                ConstantPredicateVisitor constantWithoutQuotes = new ConstantPredicateVisitor(constantVisitor.Constant.RemoveQuotes());
                return constantWithoutQuotes;
            }

            return inner;
        }

        private static bool IsDictionary(Expression expression)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return false;
            }

            if (methodCallExpression.Method.Name == "get_Item")
            {
                return true;
            }

            return false;
        }
    }
}
