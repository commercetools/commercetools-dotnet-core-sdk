using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq;
using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public static class RequestBuilderExtensions
    {
        public static List<KeyValuePair<string, string>> GetQueryStringParameters<T>(this List<Expansion<T>> expansionPredicates, IExpansionExpressionVisitor expansionExpressionVisitor)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            foreach (var expansion in expansionPredicates)
            {
                string expandPath = expansionExpressionVisitor.GetPath(expansion.Expression);
                queryStringParameters.Add(new KeyValuePair<string, string>("expand", expandPath));
            }
            return queryStringParameters;
        }

        public static List<KeyValuePair<string, string>> GetQueryStringParameters<T>(this List<Sort<T>> sortPredicates, ISortExpressionVisitor sortExpressionVisitor)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            foreach (var sort in sortPredicates)
            {
                string sortPath = sortExpressionVisitor.Render(sort.Expression);
                sortPath += GetSortDirectionPath(sort.SortDirection);
                queryStringParameters.Add(new KeyValuePair<string, string>("sort", sortPath));
            }
            return queryStringParameters;
        }

        public static List<KeyValuePair<string, string>> GetQueryStringParameters<T>(this QueryPredicate<T> queryPredicate, IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            string where = queryPredicateExpressionVisitor.ProcessExpression(queryPredicate.Expression);
            queryStringParameters.Add(new KeyValuePair<string, string>("where", where));
            return queryStringParameters;
        }

        private static string GetSortDirectionPath(SortDirection? sortDirection)
        {
            string sortPath = string.Empty;
            if (sortDirection != null)
            {
                if (sortDirection == SortDirection.Descending)
                {
                    sortPath = " desc";
                }
                else
                {
                    sortPath = " asc";
                }
            }
            return sortPath;
        }
    }
}