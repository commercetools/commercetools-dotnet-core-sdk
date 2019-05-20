using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public static class QueryCommandExtensions
    {
        public static QueryCommand<T> Where<T>(this QueryCommand<T> command, Expression<Func<T, bool>> expression)
        {
            if (command.QueryParameters is IPredicateQueryable queryParameters)
            {
                queryParameters.Where.Add(new QueryPredicate<T>(expression).ToString());
            }

            return command;
        }

        public static QueryCommand<T> Where<T>(this QueryCommand<T> command, string expression)
        {
            if (command.QueryParameters is IPredicateQueryable queryParameters)
            {
                queryParameters.Where.Add(expression);
            }

            return command;
        }

        public static QueryCommand<T> Expand<T>(this QueryCommand<T> command, Expression<Func<T, Reference>> expression)
        {
            if (command.QueryParameters is IExpandable queryParameters)
            {
                queryParameters.Expand.Add(new Expansion<T>(expression).ToString());
            }

            return command;
        }

        public static QueryCommand<T> Expand<T>(this QueryCommand<T> command, string expression)
        {
            if (command.QueryParameters is IExpandable queryParameters)
            {
                queryParameters.Expand.Add(expression);
            }

            return command;
        }

        public static QueryCommand<T> Limit<T>(this QueryCommand<T> command, int limit)
        {
            if (command.QueryParameters is IPageable queryParameters)
            {
                queryParameters.Limit = limit;
            }

            return command;
        }

        public static QueryCommand<T> Offset<T>(this QueryCommand<T> command, int offset)
        {
            if (command.QueryParameters is IPageable queryParameters)
            {
                queryParameters.Offset = offset;
            }

            return command;
        }

        public static QueryCommand<T> WithTotal<T>(this QueryCommand<T> command, bool withTotal)
        {
            if (command.QueryParameters is IPageable queryParameters)
            {
                queryParameters.WithTotal = withTotal;
            }

            return command;
        }

        public static QueryCommand<T> Sort<T>(this QueryCommand<T> command, string expression)
        {
            if (command.QueryParameters is ISortable queryParameters)
            {
                queryParameters.Sort.Add(expression);
            }

            return command;
        }

        public static QueryCommand<T> Sort<T>(this QueryCommand<T> command, Expression<Func<T, IComparable>> expression, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (command.QueryParameters is ISortable queryParameters)
            {
                queryParameters.Sort.Add(new Sort<T>(expression, sortDirection).ToString());
            }

            return command;
        }

        public static QueryCommand<T> SetExpand<T>(this QueryCommand<T> command, IEnumerable<Expansion<T>> expandPredicates)
        {
            if (command.QueryParameters is IExpandable queryParameters && expandPredicates != null)
            {
                return command.SetExpand(expandPredicates.Select(predicate => predicate.ToString()));
            }

            return command;
        }

        public static QueryCommand<T> SetExpand<T>(this QueryCommand<T> command, IEnumerable<string> expandPredicates)
        {
            if (command.QueryParameters is IExpandable queryParameters && expandPredicates != null)
            {
                queryParameters.Expand.Clear();
                foreach (var expand in expandPredicates)
                {
                    queryParameters.Expand.Add(expand);
                }
            }

            return command;
        }

        public static QueryCommand<T> SetWhere<T>(this QueryCommand<T> command, IEnumerable<QueryPredicate<T>> queryPredicates)
        {
            if (command.QueryParameters is IPredicateQueryable queryParameters && queryPredicates != null)
            {
                return command.SetWhere(queryPredicates);
            }

            return command;
        }

        public static QueryCommand<T> SetWhere<T>(this QueryCommand<T> command, IEnumerable<string> queryPredicates)
        {
            if (command.QueryParameters is IPredicateQueryable queryParameters && queryPredicates != null)
            {
                queryParameters.Where.Clear();
                foreach (var query in queryPredicates)
                {
                    queryParameters.Where.Add(query);
                }
            }

            return command;
        }

        public static QueryCommand<T> SetWhere<T>(this QueryCommand<T> command, QueryPredicate<T> queryPredicate)
        {
            if (command.QueryParameters is IPredicateQueryable queryParameters && queryPredicate != null)
            {
                return command.SetWhere(queryPredicate.ToString());
            }

            return command;
        }

        public static QueryCommand<T> SetWhere<T>(this QueryCommand<T> command, string queryPredicate)
        {
            if (command.QueryParameters is IPredicateQueryable queryParameters && !string.IsNullOrEmpty(queryPredicate))
            {
                queryParameters.Where.Clear();
                queryParameters.Where.Add(queryPredicate);
            }

            return command;
        }

        public static QueryCommand<T> SetSort<T>(this QueryCommand<T> command, IEnumerable<Sort<T>> sortPredicates)
        {
            if (command.QueryParameters is ISortable queryParameters)
            {
                return command.SetSort(sortPredicates.Select(predicate => predicate.ToString()));
            }

            return command;
        }

        public static QueryCommand<T> SetSort<T>(this QueryCommand<T> command, IEnumerable<string> sortPredicates)
        {
            if (command.QueryParameters is ISortable queryParameters && sortPredicates != null)
            {
                queryParameters.Sort.Clear();
                foreach (var sort in sortPredicates)
                {
                    queryParameters.Sort.Add(sort);
                }
            }

            return command;
        }
    }
}
