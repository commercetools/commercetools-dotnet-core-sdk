using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client.Linq
{
    using System;

    [Obsolete("Experimental")]
    public static class Extensions
    {
        public static CtpQueryable<T> Query<T>(this IClient client)
        {
            return new CtpQueryable<T>(client, new QueryCommand<T>());
        }

        public static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, Reference>> expression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    GetMethodInfo(Expand, source, expression),
                    new[] { source.Expression, expression }));
        }

        private static MethodInfo GetMethodInfo<T1, T2, T3>(Func<T1, T2, T3> f, T1 unused1, T2 unused2)
        {
            return f.Method;
        }

    }
}
