using commercetools.Sdk.Linq;
using commercetools.Sdk.Util;
using System;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("customLineItems")]
    public class CustomLineItemsCartDiscountTarget : CartDiscountTarget
    {
        public string Predicate { get; set; }

        public void SetPredicate(Expression<Func<CustomLineItem, bool>> expression)
        {
            this.Predicate = ServiceLocator.Current.GetService<ICartPredicateExpressionVisitor>().Render(expression);
        }
    }
}