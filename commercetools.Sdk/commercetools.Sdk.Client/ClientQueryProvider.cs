using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Client
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain.Query;

    public class ClientQueryProvider<T> : IQueryProvider
    {
        public QueryCommand<T> Command { get; }
        private readonly IClient client;

        private IList<T> result = new List<T>();

        public ClientQueryProvider(IClient client, QueryCommand<T> command)
        {
            this.client = client;
            this.Command = command;
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
                return new ClientQueryable<TElement>(this as ClientQueryProvider<TElement>, expression);
            }

            var cmd = this.Command as QueryCommand<TElement>;
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
                case "Expand":
                    if (mc.Arguments[1] is UnaryExpression expand)
                    {
                        cmd.Expand.Add(new Expansion<TElement>(expand.Operand).ToString());
                    }
                    break;
                default:
                    break;
            }

            return new ClientQueryable<TElement>(this as ClientQueryProvider<TElement>, expression);
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

            var queryResult = this.client.ExecuteAsync(this.Command);
            var returnedSet = queryResult.Result;

            this.result = returnedSet.Results;
            return (TResult)this.result.GetEnumerator();
        }
    }
}
