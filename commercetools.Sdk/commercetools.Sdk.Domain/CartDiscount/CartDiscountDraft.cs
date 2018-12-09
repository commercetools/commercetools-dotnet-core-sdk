using commercetools.Sdk.Linq;
using commercetools.Sdk.Util;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain
{
    [Endpoint("cart-discounts")]
    public class CartDiscountDraft
    {
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public CartDiscountValue Value { get; set; }
        public string CartPredicate { get; set; }
        public CartDiscountTarget Target { get; set; }
        public string SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool RequiresDiscountCode { get; set; }
        public List<Reference> References { get; set; }
        public StackingMode StackingMode { get; set; }
        public CustomFields Custom { get; set; }

        // TODO See if this should be cart discount or cart
        public void SetCartPredicate(Expression<Func<Cart, bool>> expression)
        {
            this.CartPredicate = ServiceLocator.Current.GetService<ICartPredicateExpressionVisitor>().Render(expression);
        }
    }
}