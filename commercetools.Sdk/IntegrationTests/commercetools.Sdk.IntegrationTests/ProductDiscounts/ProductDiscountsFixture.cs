using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductDiscounts;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.ProductDiscounts
{
    public class ProductDiscountsFixture
    {
        #region DraftBuilds

        public static ProductDiscountDraft DefaultProductDiscountDraft(ProductDiscountDraft productDiscountDraft)
        {
            var randomInt = TestingUtility.RandomInt();
            productDiscountDraft.Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};
            productDiscountDraft.Key = $"Key{randomInt}";
            productDiscountDraft.IsActive = true;
            productDiscountDraft.Value = new ExternalProductDiscountValue();
            productDiscountDraft.Predicate = "1 = 1";
            productDiscountDraft.SortOrder = TestingUtility.RandomSortOrder();
            return productDiscountDraft;
        }
        public static ProductDiscountDraft DefaultProductDiscountDraftWithKey(ProductDiscountDraft draft, string key)
        {
            var productDiscountDraft = DefaultProductDiscountDraft(draft);
            productDiscountDraft.Key = key;
            return productDiscountDraft;
        }
        #endregion

        #region WithProductDiscount

        public static async Task WithProductDiscount( IClient client, Action<ProductDiscount> func)
        {
            await With(client, new ProductDiscountDraft(), DefaultProductDiscountDraft, func);
        }
        public static async Task WithProductDiscount( IClient client, Func<ProductDiscountDraft, ProductDiscountDraft> draftAction, Action<ProductDiscount> func)
        {
            await With(client, new ProductDiscountDraft(), draftAction, func);
        }

        public static async Task WithProductDiscount( IClient client, Func<ProductDiscount, Task> func)
        {
            await WithAsync(client, new ProductDiscountDraft(), DefaultProductDiscountDraft, func);
        }
        public static async Task WithProductDiscount( IClient client, Func<ProductDiscountDraft, ProductDiscountDraft> draftAction, Func<ProductDiscount, Task> func)
        {
            await WithAsync(client, new ProductDiscountDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableProductDiscount

        public static async Task WithUpdateableProductDiscount(IClient client, Func<ProductDiscount, ProductDiscount> func)
        {
            await WithUpdateable(client, new ProductDiscountDraft(), DefaultProductDiscountDraft, func);
        }

        public static async Task WithUpdateableProductDiscount(IClient client, Func<ProductDiscountDraft, ProductDiscountDraft> draftAction, Func<ProductDiscount, ProductDiscount> func)
        {
            await WithUpdateable(client, new ProductDiscountDraft(), draftAction, func);
        }

        public static async Task WithUpdateableProductDiscount(IClient client, Func<ProductDiscount, Task<ProductDiscount>> func)
        {
            await WithUpdateableAsync(client, new ProductDiscountDraft(), DefaultProductDiscountDraft, func);
        }
        public static async Task WithUpdateableProductDiscount(IClient client, Func<ProductDiscountDraft, ProductDiscountDraft> draftAction, Func<ProductDiscount, Task<ProductDiscount>> func)
        {
            await WithUpdateableAsync(client, new ProductDiscountDraft(), draftAction, func);
        }

        #endregion
    }
}