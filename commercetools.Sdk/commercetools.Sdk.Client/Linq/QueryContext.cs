using commercetools.Sdk.Linq;
using commercetools.Sdk.Registration;

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

    public class QueryContext<T> : IQueryProvider, IOrderedQueryable<T>
    {
        private readonly QueryCommand<T> command;

        private readonly Expression expression;

        private readonly IClient client = null;

        private IList<T> result = new List<T>();

        public Type ElementType => typeof(T);

        public Expression Expression => Expression.Constant(this);

        public IQueryProvider Provider => this;

        public QueryContext(QueryCommand<T> command, Expression expression = null, IClient client = null)
        {
            this.command = command;
            this.expression = expression;
            this.client = client;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (this as IQueryable).Provider.Execute<IEnumerator<T>>(this.expression);
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

            if (!(expression is MethodCallExpression mc))
            {
                return (IQueryable<TElement>)this;
            }

            var cmd = (QueryCommand<TElement>)this.command.Clone();
            switch (mc.Method.Name)
            {
                case "Where":
                    if (mc.Arguments[1] is UnaryExpression where)
                    {
                        var t = where.Operand as Expression<Func<TElement, bool>>;
                        var queryPredicate = new QueryPredicate<TElement>(t);
                        cmd.SetWhere(queryPredicate);
                    }

                    break;
                case "Take":
                    if (mc.Arguments[1] is ConstantExpression limit)
                    {
                        cmd.Limit = (int)limit.Value;
                    }

                    break;
                case "Skip":
                    if (mc.Arguments[1] is ConstantExpression offset)
                    {
                        cmd.Offset = (int)offset.Value;
                    }

                    break;
                case "OrderBy":
                case "ThenBy":
                case "OrderByDescending":
                case "ThenByDescending":
                    if (mc.Arguments[1] is UnaryExpression sort)
                    {
                        if (mc.Method.Name.StartsWith("OrderBy", StringComparison.Ordinal))
                        {
                            cmd.Sort.Clear();
                        }

                        var direction = SortDirection.Ascending;
                        if (mc.Method.Name.EndsWith("Descending", StringComparison.Ordinal))
                        {
                            direction = SortDirection.Descending;
                        }

                        var render = ServiceLocator.Current.GetService<ISortExpressionVisitor>().Render(sort.Operand);
                        cmd.Sort.Add(new Sort<T>(render, direction).ToString());
                    }

                    break;
                case "WithClient":
                    if (mc.Arguments[1] is ConstantExpression cl)
                    {
                        return new QueryContext<TElement>(cmd, expression, (IClient)cl.Value);
                    }

                    break;
                default:
                    break;
            }

            return new QueryContext<TElement>(cmd, expression, this.client);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            if (this.client == null)
            {
                throw new FieldAccessException("Client cannot be null");
            }

            var queryResult = this.client.ExecuteAsync(this.command);
            PagedQueryResult<T> returnedSet = queryResult.Result;

            this.result = returnedSet.Results;
            return (TResult)this.result.GetEnumerator();
        }
    }
}
