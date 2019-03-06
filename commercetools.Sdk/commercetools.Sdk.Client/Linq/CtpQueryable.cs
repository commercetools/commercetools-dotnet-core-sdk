namespace commercetools.Sdk.Client.Linq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class CtpQueryable<T>: IOrderedQueryable<T>
    {

        public Type ElementType => typeof(T);

        public Expression Expression { get; private set; }

        public IQueryProvider Provider { get; private set; }


        public CtpQueryable(IClient client, QueryCommand<T> command)
        {
            this.Provider = new CtpQueryProvider<T>(client, command);
            this.Expression = Expression.Constant(this);
        }

        public CtpQueryable(IQueryProvider provider, Expression expression)
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
