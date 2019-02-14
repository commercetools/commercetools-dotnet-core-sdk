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

            return source.Provider.CreateQuery<TSource>((Expression)Expression.Call((Expression)null, WithClientTSource2(typeof(TSource)), source.Expression, (Expression)Expression.Constant((object)client)));
        }

        private static MethodInfo WithClientTSource2(Type TSource)
        {
            var methodInfo = _sWithClientTSource2;
            if ((object)methodInfo == null)
            {
                methodInfo = _sWithClientTSource2 = new Func<IQueryable<object>, IClient, IQueryable<object>>(WithClient<object>).GetMethodInfo().GetGenericMethodDefinition();
            }

            return methodInfo.MakeGenericMethod(TSource);
        }
    }
}
