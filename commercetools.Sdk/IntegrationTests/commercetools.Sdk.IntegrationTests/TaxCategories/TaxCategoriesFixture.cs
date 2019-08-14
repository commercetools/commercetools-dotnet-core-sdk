using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.TaxCategories;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.TaxCategories
{
    public static class TaxCategoriesFixture
    {
        #region DraftBuilds
        public static TaxCategoryDraft DefaultTaxCategoryDraft(TaxCategoryDraft taxCategoryDraft)
        {
            var random = TestingUtility.RandomInt();
            taxCategoryDraft.Name = $"TaxCategory_{random}";
            taxCategoryDraft.Key = $"key_{random}";
            return taxCategoryDraft;
        }
        public static TaxCategoryDraft DefaultTaxCategoryDraftWithKey(TaxCategoryDraft draft, string key)
        {
            var taxCategoryDraft = DefaultTaxCategoryDraft(draft);
            taxCategoryDraft.Key = key;
            return taxCategoryDraft;
        }
        public static TaxCategoryDraft DefaultTaxCategoryDraftWithTaxRate(TaxCategoryDraft taxCategoryDraft, TaxRateDraft taxRateDraft)
        {
            var random = TestingUtility.RandomInt();
            taxCategoryDraft.Name = $"TaxCategory_{random}";
            taxCategoryDraft.Key = $"key_{random}";
            taxCategoryDraft.Rates = new List<TaxRateDraft> { taxRateDraft };
            return taxCategoryDraft;
        }

        public static TaxRateDraft GetTaxRateDraft(string name, string country, double amount, bool includeInPrice)
        {
            var taxRateDraft = new TaxRateDraft
            {
                Amount = amount,
                Country = country,
                Name = name,
                IncludedInPrice = includeInPrice
            };
            return taxRateDraft;
        }
        #endregion

        #region WithTaxCategory

        public static async Task WithTaxCategory( IClient client, Action<TaxCategory> func)
        {
            await With(client, new TaxCategoryDraft(), DefaultTaxCategoryDraft, func);
        }
        public static async Task WithTaxCategory( IClient client, Func<TaxCategoryDraft, TaxCategoryDraft> draftAction, Action<TaxCategory> func)
        {
            await With(client, new TaxCategoryDraft(), draftAction, func);
        }

        public static async Task WithTaxCategory( IClient client, Func<TaxCategory, Task> func)
        {
            await WithAsync(client, new TaxCategoryDraft(), DefaultTaxCategoryDraft, func);
        }
        public static async Task WithTaxCategory( IClient client, Func<TaxCategoryDraft, TaxCategoryDraft> draftAction, Func<TaxCategory, Task> func)
        {
            await WithAsync(client, new TaxCategoryDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableTaxCategory

        public static async Task WithUpdateableTaxCategory(IClient client, Func<TaxCategory, TaxCategory> func)
        {
            await WithUpdateable(client, new TaxCategoryDraft(), DefaultTaxCategoryDraft, func);
        }

        public static async Task WithUpdateableTaxCategory(IClient client, Func<TaxCategoryDraft, TaxCategoryDraft> draftAction, Func<TaxCategory, TaxCategory> func)
        {
            await WithUpdateable(client, new TaxCategoryDraft(), draftAction, func);
        }

        public static async Task WithUpdateableTaxCategory(IClient client, Func<TaxCategory, Task<TaxCategory>> func)
        {
            await WithUpdateableAsync(client, new TaxCategoryDraft(), DefaultTaxCategoryDraft, func);
        }
        public static async Task WithUpdateableTaxCategory(IClient client, Func<TaxCategoryDraft, TaxCategoryDraft> draftAction, Func<TaxCategory, Task<TaxCategory>> func)
        {
            await WithUpdateableAsync(client, new TaxCategoryDraft(), draftAction, func);
        }

        #endregion
    }
}
