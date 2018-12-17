using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Linq.Discount;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class ShippingMethodDraft : IDraft<ShippingMethod>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ResourceIdentifier TaxCategory { get; set; }
        public List<ZoneRateDraft> ZoneRates { get; set; }
        public bool IsDefault { get; set; }
        public string Predicate { get; set; }

        public void SetCartPredicate(Expression<Func<Cart, bool>> expression)
        {
            this.Predicate = ServiceLocator.Current.GetService<IDiscountPredicateExpressionVisitor>().Render(expression);
        }
    }
}