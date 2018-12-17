using System;
using System.Linq.Expressions;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Linq.Discount;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    [TypeMarker("customLineItems")]
    public class CustomLineItemsCartDiscountTarget : CartDiscountTarget
    {
        public string Predicate { get; set; }

        public void SetPredicate(Expression<Func<CustomLineItem, bool>> expression)
        {
            this.Predicate = ServiceLocator.Current.GetService<IDiscountPredicateExpressionVisitor>().Render(expression);
        }
    }
}