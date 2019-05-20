using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount;
using commercetools.Sdk.Linq.Filter;
using commercetools.Sdk.Linq.Query;
using commercetools.Sdk.Linq.Sort;

namespace commercetools.Sdk.Linq
{
    public static class ExpressionRenderExtensions
    {
        private static IDiscountPredicateExpressionVisitor DiscountPredicateExpressionVisitor => new DiscountPredicateExpressionVisitor();

        private static IFilterPredicateExpressionVisitor FilterPredicateExpressionVisitor => new FilterPredicateExpressionVisitor();

        private static IQueryPredicateExpressionVisitor QueryPredicateExpressionVisitor => new QueryPredicateExpressionVisitor();

        private static ISortExpressionVisitor SortExpressionVisitor => new SortExpressionVisitor();

        private static IExpansionExpressionVisitor ExpansionExpressionVisitor => new ExpansionExpressionVisitor();

        public static string RenderDiscountPredicate(this Expression expression)
        {
            return DiscountPredicateExpressionVisitor.Render(expression);
        }

        public static string RenderFilterPredicate(this Expression expression)
        {
            return FilterPredicateExpressionVisitor.Render(expression);
        }

        public static string RenderQueryPredicate(this Expression expression)
        {
            return QueryPredicateExpressionVisitor.Render(expression);
        }

        public static string RenderSort(this Expression expression)
        {
            return SortExpressionVisitor.Render(expression);
        }

        public static string RenderExpand(this Expression expression)
        {
            return ExpansionExpressionVisitor.GetPath(expression);
        }
    }
}
