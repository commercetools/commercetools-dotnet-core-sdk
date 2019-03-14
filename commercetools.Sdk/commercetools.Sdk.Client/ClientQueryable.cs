namespace commercetools.Sdk.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ClientQueryable<T>: IOrderedQueryable<T>
    {

        public Type ElementType => typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }


        public ClientQueryable(IClient client, QueryCommand<T> command)
        {
            this.Provider = new ClientQueryProvider<T>(client, command);
            this.Expression = Expression.Constant(this);
        }

        public ClientQueryable(IQueryProvider provider, Expression expression)
        {
            this.Provider = provider;
            this.Expression = expression;
        }

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
