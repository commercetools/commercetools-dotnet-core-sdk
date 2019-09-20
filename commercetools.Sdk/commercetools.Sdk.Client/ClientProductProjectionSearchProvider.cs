using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain.ProductProjections;
    using Domain.Query;
    using Linq;
    using Registration;

    public class ClientProductProjectionSearchProvider : IQueryProvider
    {
        private readonly IClient client;

        private IList<ProductProjection> result = new List<ProductProjection>();

        public ClientProductProjectionSearchProvider(IClient client, SearchProductProjectionsCommand command)
        {
            this.client = client;
            this.Command = command;
        }

        public SearchProductProjectionsCommand Command { get; }

        public IQueryable CreateQuery(Expression expression)
        {
            return (this as IQueryProvider).CreateQuery<ProductProjection>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (typeof(TElement) != typeof(ProductProjection))
            {
                throw new ArgumentException("Only " + typeof(ProductProjection).FullName + " objects are supported");
            }

            bool isMethodCallExpression = expression is MethodCallExpression;

            if (!isMethodCallExpression)
            {
                return new ClientQueryableCollection<TElement>(this as ClientQueryProvider<TElement>, expression);
            }

            var cmd = this.Command;
            MethodCallExpression mc = expression as MethodCallExpression;
            switch (mc.Method.Name)
            {
                case "RangeFacet":
                    if (mc.Arguments[1] is UnaryExpression rangeFacet)
                    {
                        var t = rangeFacet.Operand as Expression<Func<ProductProjection, bool>>;
                        cmd.RangeFacet(t);
                    }

                    break;
                case "TermFacet":
                    if (mc.Arguments[1] is UnaryExpression facet)
                    {
                        var t = facet.Operand as Expression<Func<ProductProjection, IComparable>>;
                        cmd.TermFacet(t);
                    }

                    break;
                case "FilterFacet":
                    if (mc.Arguments[1] is UnaryExpression filterFacet)
                    {
                        var t = filterFacet.Operand as Expression<Func<ProductProjection, bool>>;
                        cmd.FilterFacets(t);
                    }

                    break;
                case "Filter":
                    if (mc.Arguments[1] is UnaryExpression filter)
                    {
                        var t = filter.Operand as Expression<Func<ProductProjection, bool>>;
                        cmd.Filter(t);
                    }

                    break;
                case "FilterQuery":
                case "Where":
                    if (mc.Arguments[1] is UnaryExpression where)
                    {
                        var t = where.Operand as Expression<Func<ProductProjection, bool>>;
                        cmd.FilterQuery(t);
                    }

                    break;
                case "Take":
                    if (mc.Arguments[1] is ConstantExpression limit)
                    {
                        if (cmd.SearchParameters != null && cmd.SearchParameters is ProductProjectionSearchParameters parameters)
                        {
                            // Set limit to lowest value
                            // Case: query.Take(5).Take(10) should still yield only 5 items
                            if (parameters.Limit == null)
                            {
                                parameters.Limit = (int)limit.Value;
                            }
                            else
                            {
                                parameters.Limit = Math.Min(parameters.Limit.Value, (int)limit.Value);
                            }
                        }
                    }

                    break;
                case "Skip":
                    if (mc.Arguments[1] is ConstantExpression offset)
                    {
                        if (cmd.SearchParameters != null && cmd.SearchParameters is ProductProjectionSearchParameters parameters)
                        {
                            // Sum all skips together
                            // Case: query.Skip(5).Skip(10) should result in an offset of 15
                            if (parameters.Offset == null)
                            {
                                parameters.Offset = (int)offset.Value;
                            }
                            else
                            {
                                parameters.Offset += (int)offset.Value;
                            }

                            // Case: query.Take(3).Skip(2) should yield only 1 item
                            // Case: query.Take(2).Skip(1).Take(2) should yield 0 items
                            // Case: query.Skip(3).Take(1).Skip(1) should yield 0 items
                            // This case is only of relevance if at least one Take operation was done beforehand
                            if (parameters.Limit != null)
                            {
                                parameters.Limit -= (int)offset.Value;
                                parameters.Limit = Math.Max(0, parameters.Limit.Value);
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
                        if (cmd.SearchParameters != null && cmd.SearchParameters is ISortable parameters)
                        {
                            if (mc.Method.Name.StartsWith("OrderBy", StringComparison.Ordinal))
                            {
                                parameters.Sort.Clear();
                            }

                            var direction = SortDirection.Ascending;
                            if (mc.Method.Name.EndsWith("Descending", StringComparison.Ordinal))
                            {
                                direction = SortDirection.Descending;
                            }

                            var render = sort.Operand.RenderSort();
                            parameters.Sort.Add(new Sort<ProductProjection>(render, direction).ToString());
                        }
                    }

                    break;
                case "Expand":
                    if (mc.Arguments[1] is UnaryExpression expand)
                    {
                        if (cmd.SearchParameters != null && cmd.SearchParameters is IExpandable parameters)
                        {
                            parameters.Expand.Add(new Expansion<ProductProjection>(expand.Operand).ToString());
                        }
                    }

                    break;
                default:
                    break;
            }

            return new ClientQueryableCollection<TElement>(this, expression);
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
