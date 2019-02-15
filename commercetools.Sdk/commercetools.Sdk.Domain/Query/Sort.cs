using commercetools.Sdk.Linq;
using System;
using System.Linq.Expressions;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Domain.Query
{
    public class Sort<T>
    {
        public Expression<Func<T, IComparable>> Expression { get; set; }
        public SortDirection? SortDirection { get; set; }

        private readonly string sortPath;

        public Sort(Expression<Func<T, IComparable>> expression, SortDirection sortDirection = Query.SortDirection.Ascending)
            : this(ServiceLocator.Current.GetService<ISortExpressionVisitor>().Render(expression), sortDirection)
        {
            this.Expression = expression;
        }

        public Sort(string path, SortDirection sortDirection)
        {
            this.sortPath = path;
            this.SortDirection = sortDirection;
        }

        public override string ToString()
        {
            string path = this.sortPath;
            if (this.SortDirection != null)
            {
                path += $" {this.SortDirection.GetDescription()}";
            }
            return path;
        }
    }
}
