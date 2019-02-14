using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Linq;

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
        private readonly QueryCommand<T> command = new QueryCommand<T>();

        private Expression expression = null;

        private IList<T> result = new List<T>();

        public Type ElementType => typeof(T);

        public Expression Expression => Expression.Constant(this);

        public IQueryProvider Provider => this;

        private IClient Client { get; set; }

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

            this.expression = expression;
            if (expression is MethodCallExpression mc)
            {
                switch (mc.Method.Name)
                {
                    case "Where":
                        if (mc.Arguments[1] is UnaryExpression where)
                        {
                            var t = where.Operand as Expression<Func<T, bool>>;
                            var queryPredicate = new QueryPredicate<T>(t);
                            this.command.SetWhere(queryPredicate);
                        }

                        break;
                    case "Take":
                        if (mc.Arguments[1] is ConstantExpression limit)
                        {
                            this.command.Limit = (int)limit.Value;
                        }

                        break;
                    case "Skip":
                        if (mc.Arguments[1] is ConstantExpression offset)
                        {
                            this.command.Offset = (int)offset.Value;
                        }

                        break;
                    case "OrderBy":
                    case "ThenBy":
                        if (mc.Arguments[1] is UnaryExpression sort)
                        {
                            if (mc.Method.Name == "OrderBy")
                            {
                                this.command.Sort.Clear();
                            }

                            ISortExpressionVisitor sortVisitor = new SortExpressionVisitor();
                            var render = sortVisitor.Render(sort.Operand);
                            this.command.Sort.Add(render);
                        }

                        break;
                    case "WithClient":
                        if (mc.Arguments[1] is ConstantExpression cl)
                        {
                            this.Client = (IClient)cl.Value;
                        }

                        break;
                    default:
                        break;
                }
            }

            return (IQueryable<TElement>)this;
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var result = Client.ExecuteAsync(command);
            PagedQueryResult<T> returnedSet = result.Result;

            this.result = returnedSet.Results;
            return (TResult)this.result.GetEnumerator();
        }
    }
}
