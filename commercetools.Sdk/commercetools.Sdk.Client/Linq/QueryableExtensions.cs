using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace commercetools.Sdk.Client.Linq
{
    public static class QueryableExtensions
    {
        private static MethodInfo _sWithClientTSource2;

        public static IQueryable<TSource> WithClient<TSource>(this IQueryable<TSource> source, IClient client)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    GetMethodInfo(WithClient, source, client),
                    new[] { source.Expression, Expression.Constant(client) }));
        }

        private static MethodInfo GetMethodInfo<T1, T2, T3>(Func<T1, T2, T3> f, T1 unused1, T2 unused2)
        {
            return f.Method;
        }
    }
}
