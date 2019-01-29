using System;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.ShoppingLists;

namespace commercetools.Sdk.Domain.Query
{
    public class LineItemExpansion<T> : Expansion<T, LineItem> where T : ShoppingList
    {
        public LineItemExpansion(Expression<Func<T, LineItem>> expression) : base(expression)
        {
        }
    }
}
