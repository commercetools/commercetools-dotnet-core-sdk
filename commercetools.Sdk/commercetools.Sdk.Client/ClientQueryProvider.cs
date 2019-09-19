using System.Collections.Generic;
using commercetools.Sdk.Domain;
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
        private readonly IClient client;

        private IList<T> result = new List<T>();

        public ClientQueryProvider(IClient client, QueryCommand<T> command)
        {
            this.client = client;
            this.Command = command;
        }

        public QueryCommand<T> Command { get; }

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

            bool isMethodCallExpression = expression is MethodCallExpression;

            if (!isMethodCallExpression)
            {
                return new ClientQueryableCollection<TElement>(this as ClientQueryProvider<TElement>, expression);
            }

            var cmd = this.Command as QueryCommand<TElement>;
            MethodCallExpression mc = expression as MethodCallExpression;
            switch (mc.Method.Name)
            {
                case "Where":
                    if (mc.Arguments[1] is UnaryExpression where)
                    {
                        var t = where.Operand as Expression<Func<TElement, bool>>;
                        var queryPredicate = new QueryPredicate<TElement>(t);
                        if (cmd != null && cmd.QueryParameters is IPredicateQueryable queryParameters)
                        {
                            queryParameters.Where.Add(queryPredicate.ToString());
                        }
                    }

                    break;
                case "Take":
                    if (mc.Arguments[1] is ConstantExpression limit)
                    {
                        if (cmd != null && cmd.QueryParameters is IPageable queryParameters)
                        {
                            // Set limit to lowest value
                            // Case: query.Take(5).Take(10) should still yield only 5 items
                            if (queryParameters.Limit == null)
                            {
                                queryParameters.Limit = (int)limit.Value;
                            }
                            else
                            {
                                queryParameters.Limit = Math.Min(queryParameters.Limit.Value, (int)limit.Value);
                            }
                        }
                    }

                    break;
                case "Skip":
                    if (mc.Arguments[1] is ConstantExpression offset)
                    {
                        if (cmd != null && cmd.QueryParameters is IPageable queryParameters)
                        {
                            // Sum all skips together
                            // Case: query.Skip(5).Skip(10) should result in an offset of 15
                            if (queryParameters.Offset == null)
                            {
                                queryParameters.Offset = (int)offset.Value;
                            }
                            else
                            {
                                queryParameters.Offset += (int)offset.Value;
                            }

                            // Case: query.Take(3).Skip(2) should yield only 1 item
                            // Case: query.Take(2).Skip(1).Take(2) should yield 0 items
                            // Case: query.Skip(3).Take(1).Skip(1) should yield 0 items
                            // This case is only of relevance if at least one Take operation was done beforehand
                            if (queryParameters.Limit != null)
                            {
                                queryParameters.Limit -= (int)offset.Value;
                                queryParameters.Limit = Math.Max(0, queryParameters.Limit.Value);
                            }
                        }
                    }

                    break;
                case "OrderBy":
                case "ThenBy":
                case "OrderByDescending":
                case "ThenByDescending":
                    if (mc.Arguments[1] is UnaryExpression sort)
                    {
                        if (cmd != null && cmd.QueryParameters is ISortable queryParameters)
                        {
                            if (mc.Method.Name.StartsWith("OrderBy", StringComparison.Ordinal))
                            {
                                queryParameters.Sort.Clear();
                            }

                            var direction = SortDirection.Ascending;
                            if (mc.Method.Name.EndsWith("Descending", StringComparison.Ordinal))
                            {
                                direction = SortDirection.Descending;
                            }

                            var render = sort.Operand.RenderSort();
                            queryParameters.Sort.Add(new Sort<T>(render, direction).ToString());
                        }
                    }

                    break;
                case "Expand":
                    if (mc.Arguments[1] is UnaryExpression expand)
                    {
                        if (cmd != null && cmd.QueryParameters is IExpandable queryParameters)
                        {
                            queryParameters.Expand.Add(new Expansion<TElement>(expand.Operand).ToString());
                        }
                    }

                    break;
                default:
                    break;
            }

            return new ClientQueryableCollection<TElement>(this as ClientQueryProvider<TElement>, expression);
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
