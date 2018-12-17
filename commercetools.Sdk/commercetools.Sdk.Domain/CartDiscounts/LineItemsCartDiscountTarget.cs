using commercetools.Sdk.Domain.Carts;
using System;
using System.Linq.Expressions;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Linq.Discount;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    [TypeMarker("lineItems")]
    public class LineItemsCartDiscountTarget : CartDiscountTarget
    {
        public string Predicate { get; set; }

        public void SetPredicate(Expression<Func<LineItem, bool>> expression)
        {
            this.Predicate = ServiceLocator.Current.GetService<IDiscountPredicateExpressionVisitor>().Render(expression);
        }
    }
}