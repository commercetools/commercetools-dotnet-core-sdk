using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public static class CheckByQueryCommandExtensions
    {
        public static CheckByQueryCommand<T> Where<T>(this CheckByQueryCommand<T> command, Expression<Func<T, bool>> expression)
        where T : Resource<T>, ICheckable<T>
        {
            command.Where.Add(new QueryPredicate<T>(expression).ToString());
            return command;
        }

        public static CheckByQueryCommand<T> Where<T>(this CheckByQueryCommand<T> command, string expression)
            where T : Resource<T>, ICheckable<T>
        {
            command.Where.Add(expression);
            return command;
        }

        public static CheckByQueryCommand<T> SetWhere<T>(this CheckByQueryCommand<T> command, IEnumerable<QueryPredicate<T>> queryPredicates)
            where T : Resource<T>, ICheckable<T>
        {
            return command.SetWhere(queryPredicates.Select(predicate => predicate.ToString()));
        }

        public static CheckByQueryCommand<T> SetWhere<T>(this CheckByQueryCommand<T> command, IEnumerable<string> queryPredicates)
            where T : Resource<T>, ICheckable<T>
        {
            command.Where.Clear();
            foreach (var query in queryPredicates)
            {
                command.Where.Add(query);
            }

            return command;
        }

        public static CheckByQueryCommand<T> SetWhere<T>(this CheckByQueryCommand<T> command, QueryPredicate<T> queryPredicate)
            where T : Resource<T>, ICheckable<T>
        {
            return command.SetWhere(queryPredicate.ToString());
        }

        public static CheckByQueryCommand<T> SetWhere<T>(this CheckByQueryCommand<T> command, string queryPredicate)
            where T : Resource<T>, ICheckable<T>
        {
            command.Where.Clear();
            command.Where.Add(queryPredicate);
            return command;
        }
    }
}
