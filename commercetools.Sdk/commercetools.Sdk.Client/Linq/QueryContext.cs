namespace commercetools.Sdk.Client.Linq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain;
    using Domain.Query;
    using Type = System.Type;

    public class QueryContext<T> : IQueryable<T>, IQueryProvider
    {
        private readonly IClient client;
        private Expression expression = null;
        private IList<T> result = new List<T>();

        public QueryContext(IClient client)
        {
            this.client = client;
        }

        public Type ElementType => typeof(T);

        public Expression Expression => Expression.Constant(this);

        public IQueryProvider Provider => this;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (this as IQueryable).Provider.Execute<IEnumerator<T>>(expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return (this as IQueryProvider).CreateQuery<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (typeof(TElement) != typeof(T))
            {
                throw new ArgumentException("Only " + typeof(T).FullName + " objects are supported");
            }

            this.expression = expression;

            return (IQueryable<TElement>)this;
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }


        public TResult Execute<TResult>(Expression expression)
        {

            QueryCommand<T> queryCommand = new QueryCommand<T>();
            if (expression is Expression<Func<T, bool>> expression1)
            {
                var queryPredicate = new QueryPredicate<T>(expression1);
                queryCommand.SetWhere(queryPredicate);
            }

            PagedQueryResult<T> returnedSet = client.ExecuteAsync(queryCommand).Result;

            this.result = returnedSet.Results;
            return (TResult)this.result.GetEnumerator();
        }
    }
}
