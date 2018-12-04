using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class QueryStringRequestBuilderFactory : IQueryStringRequestBuilderFactory
    {
        private IEnumerable<IQueryStringRequestBuilder> queryStringRequestBuilders;

        public QueryStringRequestBuilderFactory(IEnumerable<IQueryStringRequestBuilder> queryStringRequestBuilders)
        {
            this.queryStringRequestBuilders = queryStringRequestBuilders;
        }

        public IQueryStringRequestBuilder<T> GetQueryStringRequestBuilder<T>()
        {
            foreach(IQueryStringRequestBuilder queryStringRequestBuilder in this.queryStringRequestBuilders)
            {
                Type type = queryStringRequestBuilder.GetType();
                var genericType = type.GetInterfaces().First().GetGenericArguments().First();
                if (genericType == typeof(T))
                {
                    return (IQueryStringRequestBuilder<T>)queryStringRequestBuilder;
                }
            }
            return null;
        }
    }
}
