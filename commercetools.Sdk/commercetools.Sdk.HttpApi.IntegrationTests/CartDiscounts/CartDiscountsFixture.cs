using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.ProductDiscounts;

namespace commercetools.Sdk.HttpApi.IntegrationTests.CartDiscounts
{
    public class CartDiscountsFixture : ClientFixture, IDisposable
    {
        public List<CartDiscount> CartDiscountsToDelete { get; }

        public CartDiscountsFixture() : base()
        {
            this.CartDiscountsToDelete = new List<CartDiscount>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.CartDiscountsToDelete.Reverse();
            foreach (CartDiscount cartDiscount in this.CartDiscountsToDelete)
            {
                CartDiscount deletedType = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<CartDiscount>(new Guid(cartDiscount.Id),
                        cartDiscount.Version)).Result;
            }
        }

        public CartDiscountDraft GetCartDiscountDraft()
        {
            CartDiscountDraft cartDiscountDraft = new CartDiscountDraft();
            cartDiscountDraft.Name = new LocalizedString() {{"en", this.RandomString(10)}};
            cartDiscountDraft.Description = new LocalizedString() {{"en", this.RandomString(20)}};
            cartDiscountDraft.Value = this.GetCartDiscountValueAsAbsolute();
            cartDiscountDraft.CartPredicate = "1 = 1"; //match all carts
            cartDiscountDraft.SortOrder = this.RandomSortOrder();
            cartDiscountDraft.Target = GetCartDiscountTargetAsLineItems();
            cartDiscountDraft.ValidFrom = DateTime.Today.AddMonths(this.RandomInt(-5, -1));
            cartDiscountDraft.ValidUntil = DateTime.Today.AddMonths(this.RandomInt(1, 5));
            cartDiscountDraft.IsActive = true;
            cartDiscountDraft.StackingMode = StackingMode.Stacking;
            cartDiscountDraft.RequiresDiscountCode = false;
            return cartDiscountDraft;
        }

        public CartDiscount CreateCartDiscount()
        {
            return this.CreateCartDiscount(this.GetCartDiscountDraft());
        }

        public CartDiscount CreateCartDiscount(CartDiscountDraft cartDiscountDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            CartDiscount cartDiscount = commerceToolsClient.ExecuteAsync(new CreateCommand<CartDiscount>(cartDiscountDraft)).Result;
            return cartDiscount;
        }

        public AbsoluteCartDiscountValue GetCartDiscountValueAsAbsolute()
        {
            var money = new Money()
            {
                CurrencyCode = "EUR",
                CentAmount = this.RandomInt(100, 1000)
            };
            var cartDiscountValue = new AbsoluteCartDiscountValue()
            {
                Money = new List<Money>() {money}
            };
            return cartDiscountValue;
        }

        public LineItemsCartDiscountTarget GetCartDiscountTargetAsLineItems()
        {
            var discountTarget = new LineItemsCartDiscountTarget();
            discountTarget.Predicate = "1 = 1"; // target any line item
            //discountTarget.SetPredicate(lineItem => lineItem.);
            return discountTarget;
        }
    }
}
