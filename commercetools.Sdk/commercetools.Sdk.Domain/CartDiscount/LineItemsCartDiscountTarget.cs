using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Util;
using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("lineItems")]
    public class LineItemsCartDiscountTarget : CartDiscountTarget
    {
        public string Predicate { get; set; }

        public void SetPredicate(Expression<Func<LineItem, bool>> expression)
        {
            this.Predicate = ServiceLocator.Current.GetService<ICartPredicateExpressionVisitor>().Render(expression);
        }
    }
}