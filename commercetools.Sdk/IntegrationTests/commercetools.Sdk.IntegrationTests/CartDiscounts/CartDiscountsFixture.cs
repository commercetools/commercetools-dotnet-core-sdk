using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.CartDiscounts;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using Type = commercetools.Sdk.Domain.Types.Type;

namespace commercetools.Sdk.IntegrationTests.CartDiscounts
{
    public static class CartDiscountsFixture
    {
        #region DraftBuilds
        public static CartDiscountDraft DefaultCartDiscountDraft(CartDiscountDraft cartDiscountDraft)
        {
            var random = TestingUtility.RandomInt();
            cartDiscountDraft.Key = $"Key_{random}";
            cartDiscountDraft.Name = new LocalizedString() {{"en", $"CartDiscount_{random}"}};
            cartDiscountDraft.Value = TestingUtility.GetRandomAbsoluteCartDiscountValue();
            cartDiscountDraft.Target = new LineItemsCartDiscountTarget { Predicate = "1 = 1" };//match all line items

            cartDiscountDraft.CartPredicate = "1 = 1"; //match all carts
            cartDiscountDraft.SortOrder = TestingUtility.RandomSortOrder();
            cartDiscountDraft.ValidFrom = DateTime.Today;
            cartDiscountDraft.ValidUntil = DateTime.Today.AddDays(10);
            cartDiscountDraft.IsActive = true;
            cartDiscountDraft.StackingMode = StackingMode.Stacking;
            cartDiscountDraft.RequiresDiscountCode = false;
            return cartDiscountDraft;
        }
        public static CartDiscountDraft DefaultCartDiscountDraftWithKey(CartDiscountDraft draft, string key)
        {
            var cartDiscountDraft = DefaultCartDiscountDraft(draft);
            cartDiscountDraft.Key = key;
            return cartDiscountDraft;
        }
        public static CartDiscountDraft DefaultCartDiscountDraftRequireDiscountCode(CartDiscountDraft draft)
        {
            var cartDiscountDraft = DefaultCartDiscountDraft(draft);
            cartDiscountDraft.RequiresDiscountCode = true;
            return cartDiscountDraft;
        }
        public static CartDiscountDraft DefaultCartDiscountDraftWithCustomType(CartDiscountDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };

            var cartDiscountDraft = DefaultCartDiscountDraft(draft);
            cartDiscountDraft.Custom = customFieldsDraft;

            return cartDiscountDraft;
        }
        #endregion

        #region WithCartDiscount

        public static async Task WithCartDiscount( IClient client, Action<CartDiscount> func)
        {
            await With(client, new CartDiscountDraft(), DefaultCartDiscountDraft, func);
        }
        public static async Task WithCartDiscount( IClient client, Func<CartDiscountDraft, CartDiscountDraft> draftAction, Action<CartDiscount> func)
        {
            await With(client, new CartDiscountDraft(), draftAction, func);
        }

        public static async Task WithCartDiscount( IClient client, Func<CartDiscount, Task> func)
        {
            await WithAsync(client, new CartDiscountDraft(), DefaultCartDiscountDraft, func);
        }
        public static async Task WithCartDiscount( IClient client, Func<CartDiscountDraft, CartDiscountDraft> draftAction, Func<CartDiscount, Task> func)
        {
            await WithAsync(client, new CartDiscountDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableCartDiscount

        public static async Task WithUpdateableCartDiscount(IClient client, Func<CartDiscount, CartDiscount> func)
        {
            await WithUpdateable(client, new CartDiscountDraft(), DefaultCartDiscountDraft, func);
        }

        public static async Task WithUpdateableCartDiscount(IClient client, Func<CartDiscountDraft, CartDiscountDraft> draftAction, Func<CartDiscount, CartDiscount> func)
        {
            await WithUpdateable(client, new CartDiscountDraft(), draftAction, func);
        }

        public static async Task WithUpdateableCartDiscount(IClient client, Func<CartDiscount, Task<CartDiscount>> func)
        {
            await WithUpdateableAsync(client, new CartDiscountDraft(), DefaultCartDiscountDraft, func);
        }
        public static async Task WithUpdateableCartDiscount(IClient client, Func<CartDiscountDraft, CartDiscountDraft> draftAction, Func<CartDiscount, Task<CartDiscount>> func)
        {
            await WithUpdateableAsync(client, new CartDiscountDraft(), draftAction, func);
        }

        #endregion
    }
}
