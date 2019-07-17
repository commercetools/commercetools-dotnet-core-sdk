using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Common;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.DiscountCodes
{
    public static class DiscountCodesFixture
    {
        #region DraftBuilds
        public static DiscountCodeDraft DefaultDiscountCodeDraft(DiscountCodeDraft discountCodeDraft)
        {
            var random = TestingUtility.RandomInt(1, 100);
            discountCodeDraft.Name = new LocalizedString {{"en", $"DiscountCode_{random}"}};
            discountCodeDraft.Code = TestingUtility.RandomString();
            discountCodeDraft.IsActive = true;
            discountCodeDraft.CartPredicate = "1 = 1"; //for all carts
            discountCodeDraft.ValidFrom = DateTime.Today;
            discountCodeDraft.ValidUntil = DateTime.Today.AddDays(1); // valid one day
            return discountCodeDraft;
        }
        public static DiscountCodeDraft DefaultDiscountCodeDraftWithCode(DiscountCodeDraft draft, string code)
        {
            var discountCodeDraft = DefaultDiscountCodeDraft(draft);
            discountCodeDraft.Code = code;
            return discountCodeDraft;
        }

        #endregion

        #region WithDiscountCode

        public static async Task WithDiscountCode( IClient client, Action<DiscountCode> func)
        {
            await With(client, new DiscountCodeDraft(), DefaultDiscountCodeDraft, func);
        }
        public static async Task WithDiscountCode( IClient client, Func<DiscountCodeDraft, DiscountCodeDraft> draftAction, Action<DiscountCode> func)
        {
            await With(client, new DiscountCodeDraft(), draftAction, func);
        }

        public static async Task WithDiscountCode( IClient client, Func<DiscountCode, Task> func)
        {
            await WithAsync(client, new DiscountCodeDraft(), DefaultDiscountCodeDraft, func);
        }
        public static async Task WithDiscountCode( IClient client, Func<DiscountCodeDraft, DiscountCodeDraft> draftAction, Func<DiscountCode, Task> func)
        {
            await WithAsync(client, new DiscountCodeDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableDiscountCode

        public static async Task WithUpdateableDiscountCode(IClient client, Func<DiscountCode, DiscountCode> func)
        {
            await WithUpdateable(client, new DiscountCodeDraft(), DefaultDiscountCodeDraft, func);
        }

        public static async Task WithUpdateableDiscountCode(IClient client, Func<DiscountCodeDraft, DiscountCodeDraft> draftAction, Func<DiscountCode, DiscountCode> func)
        {
            await WithUpdateable(client, new DiscountCodeDraft(), draftAction, func);
        }

        public static async Task WithUpdateableDiscountCode(IClient client, Func<DiscountCode, Task<DiscountCode>> func)
        {
            await WithUpdateableAsync(client, new DiscountCodeDraft(), DefaultDiscountCodeDraft, func);
        }
        public static async Task WithUpdateableDiscountCode(IClient client, Func<DiscountCodeDraft, DiscountCodeDraft> draftAction, Func<DiscountCode, Task<DiscountCode>> func)
        {
            await WithUpdateableAsync(client, new DiscountCodeDraft(), draftAction, func);
        }

        #endregion
    }
}
