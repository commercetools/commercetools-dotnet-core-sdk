using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public abstract class PredicateExpressionVisitorBase : IPredicateExpressionVisitor
    {
        private readonly IPredicateVisitorFactory predicateVisitorFactory;

        protected PredicateExpressionVisitorBase(IPredicateVisitorFactory predicateVisitorFactory)
        {
            this.predicateVisitorFactory = predicateVisitorFactory;
        }

        public string Render(Expression expression)
        {
            IPredicateVisitor predicateVisitor = this.predicateVisitorFactory.Create(expression);
            return predicateVisitor.Render();
        }
    }
}