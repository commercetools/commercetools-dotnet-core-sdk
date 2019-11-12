using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using Type = commercetools.Sdk.Domain.Types.Type;

namespace commercetools.Sdk.IntegrationTests.Categories
{
    public class CategoriesFixture
    {
         #region DraftBuilds

        public static CategoryDraft DefaultCategoryDraft(CategoryDraft categoryDraft)
        {
            var randomInt = TestingUtility.RandomInt();
            categoryDraft.Name = new LocalizedString {{"en", $"Name{randomInt}"}};
            categoryDraft.Slug = new LocalizedString {{"en", $"Slug{randomInt}"}};
            categoryDraft.Key = $"Key{randomInt}";
            categoryDraft.OrderHint = TestingUtility.RandomSortOrder();
            return categoryDraft;
        }
        public static CategoryDraft DefaultCategoryDraftWithParent(CategoryDraft draft, Category parent)
        {
            var categoryDraft = DefaultCategoryDraft(draft);
            categoryDraft.Parent = parent.ToReference();
            return categoryDraft;
        }
        public static CategoryDraft DefaultCategoryDraftWithAsset(CategoryDraft draft)
        {
            var categoryDraft = DefaultCategoryDraft(draft);
            var assetDraft = TestingUtility.GetAssetDraft();
            categoryDraft.Assets = new List<AssetDraft> { assetDraft };
            return categoryDraft;
        }
        public static CategoryDraft DefaultCategoryDraftWithAssetWithCustomType(CategoryDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };
            var categoryDraft = DefaultCategoryDraft(draft);
            var assetDraft = TestingUtility.GetAssetDraft();
            assetDraft.Custom = customFieldsDraft;
            categoryDraft.Assets = new List<AssetDraft> { assetDraft };
            return categoryDraft;
        }
        public static CategoryDraft DefaultCategoryDraftWithMultipleAssets(CategoryDraft draft, int assetsCount = 3)
        {
            var categoryDraft = DefaultCategoryDraft(draft);
            categoryDraft.Assets = new List<AssetDraft>();
            for (int i = 1; i <= assetsCount; i++)
            {
                var assetDraft = TestingUtility.GetAssetDraft();
                categoryDraft.Assets.Add(assetDraft);
            }
            return categoryDraft;
        }
        public static CategoryDraft DefaultCategoryDraftWithKey(CategoryDraft draft, string key)
        {
            var categoryDraft = DefaultCategoryDraft(draft);
            categoryDraft.Key = key;
            return categoryDraft;
        }
        public static CategoryDraft DefaultCategoryDraftWithCustomType(CategoryDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };

            var customerDraft = DefaultCategoryDraft(draft);
            customerDraft.Custom = customFieldsDraft;

            return customerDraft;
        }
        #endregion

        #region WithCategory

        public static async Task WithCategory( IClient client, Action<Category> func)
        {
            await With(client, new CategoryDraft(), DefaultCategoryDraft, func);
        }
        public static async Task WithCategory( IClient client, Func<CategoryDraft, CategoryDraft> draftAction, Action<Category> func)
        {
            await With(client, new CategoryDraft(), draftAction, func);
        }

        public static async Task WithCategory( IClient client, Func<Category, Task> func)
        {
            await WithAsync(client, new CategoryDraft(), DefaultCategoryDraft, func);
        }
        public static async Task WithCategory( IClient client, Func<CategoryDraft, CategoryDraft> draftAction, Func<Category, Task> func)
        {
            await WithAsync(client, new CategoryDraft(), draftAction, func);
        }
        public static async Task WithListOfCategories( IClient client, Func<CategoryDraft, CategoryDraft> draftAction, int count, Func<List<Category>, Task> func)
        {
            await WithListAsync(client, new CategoryDraft(), draftAction, func, count);
        }
        #endregion

        #region WithUpdateableCategory

        public static async Task WithUpdateableCategory(IClient client, Func<Category, Category> func)
        {
            await WithUpdateable(client, new CategoryDraft(), DefaultCategoryDraft, func);
        }

        public static async Task WithUpdateableCategory(IClient client, Func<CategoryDraft, CategoryDraft> draftAction, Func<Category, Category> func)
        {
            await WithUpdateable(client, new CategoryDraft(), draftAction, func);
        }

        public static async Task WithUpdateableCategory(IClient client, Func<Category, Task<Category>> func)
        {
            await WithUpdateableAsync(client, new CategoryDraft(), DefaultCategoryDraft, func);
        }
        public static async Task WithUpdateableCategory(IClient client, Func<CategoryDraft, CategoryDraft> draftAction, Func<Category, Task<Category>> func)
        {
            await WithUpdateableAsync(client, new CategoryDraft(), draftAction, func);
        }

        #endregion
    }
}

