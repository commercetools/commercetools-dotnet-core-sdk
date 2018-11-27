using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain
{
    public class QueryPredicate<T>
    {
        public Expression<Func<T, bool>> Expression { get; set; }

        public QueryPredicate(Expression<Func<T, bool>> expression)
        {
            this.Expression = expression;
        }
    }

    public static class QueryExtensions
    {
        public static bool In<T>(this T source, params T[] values)
        {
            return values.Contains(source);
        }

        public static bool NotIn<T>(this T source, params T[] values)
        {
            return !values.Contains(source);
        }

        public static bool ContainsAll<T>(this IEnumerable<T> containingList, params T[] lookupList)
        {
            return !lookupList.Except(containingList).Any();
        }

        public static bool Equals<T>(this T something, Func<T, T> p)
        {
            throw new NotImplementedException();
        }
    }
}