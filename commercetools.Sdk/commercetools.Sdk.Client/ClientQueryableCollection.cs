using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;

namespace commercetools.Sdk.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ClientQueryableCollection<T> : IOrderedQueryable<T>
    {
        public ClientQueryableCollection(IClient client, QueryCommand<T> command, bool queryAll = false)
        {
            this.Provider = new ClientQueryProvider<T>(client, command, queryAll);
            this.Expression = Expression.Constant(this);
        }

        public ClientQueryableCollection(IClient client, SearchProductProjectionsCommand command)
        {
            this.Provider = new ClientProductProjectionSearchProvider(client, command);
            this.Expression = Expression.Constant(this);
        }

        public ClientQueryableCollection(IQueryProvider provider, Expression expression)
        {
            this.Provider = provider;
            this.Expression = expression;
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Provider.Execute<IEnumerator<T>>(this.Expression);
        }
    }
}
