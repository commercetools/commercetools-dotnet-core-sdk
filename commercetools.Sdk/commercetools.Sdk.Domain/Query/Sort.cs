using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain
{
    public class Sort<T>
    {
        public Expression<Func<T, IComparable>> Expression { get; set; }
        public SortDirection? SortDirection { get; set; }

        public Sort(Expression<Func<T, IComparable>> expression)
        {
            this.Expression = expression;
        }

        public Sort(Expression<Func<T, IComparable>> expression, SortDirection sortDirection)
        {
            this.Expression = expression;
            this.SortDirection = sortDirection;
        }
    }

    public enum SortDirection
    {
        Ascending, Descending
    }
}