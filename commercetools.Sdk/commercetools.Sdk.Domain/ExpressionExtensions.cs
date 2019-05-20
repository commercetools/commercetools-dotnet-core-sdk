using System;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Linq.Discount;
using commercetools.Sdk.Linq.Filter;
using commercetools.Sdk.Linq.Query;
using commercetools.Sdk.Linq.Sort;
using commercetools.Sdk.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Domain
{
    public static class ExpressionExtensions
    {
        public static void SetPredicate(this ProductDiscountDraft productDiscountDraft, Expression<Func<Product, bool>> expression)
        {
            productDiscountDraft.Predicate = expression.RenderDiscountPredicate();
        }

        public static void SetCartPredicate(this CartDiscountDraft cartDiscountDraft, Expression<Func<Cart, bool>> expression)
        {
            cartDiscountDraft.CartPredicate = expression.RenderDiscountPredicate();
        }

        public static void SetPredicate(this CustomLineItemsCartDiscountTarget customLineItemsCartDiscountTarget, Expression<Func<CustomLineItem, bool>> expression)
        {
            customLineItemsCartDiscountTarget.Predicate = expression.RenderDiscountPredicate();
        }

        public static void SetPredicate(this LineItemsCartDiscountTarget lineItemsCartDiscountTarget, Expression<Func<LineItem, bool>> expression)
        {
            lineItemsCartDiscountTarget.Predicate = expression.RenderDiscountPredicate();
        }

        public static void SetCartPredicate(this ShippingMethodDraft shippingMethodDraft, Expression<Func<Cart, bool>> expression)
        {
            shippingMethodDraft.Predicate = expression.RenderDiscountPredicate();
        }


    }
}
