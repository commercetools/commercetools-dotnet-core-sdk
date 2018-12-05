using commercetools.Sdk.Linq;
using commercetools.Sdk.Util;
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

        public override string ToString()
        {
            string sortPath = ServiceLocator.Current.GetService<ISortExpressionVisitor>().Render(this.Expression);
            if (this.SortDirection != null)
            {
                sortPath += $" {this.SortDirection.GetDescription()}";
            }
            return sortPath;
        }
    }

    
}