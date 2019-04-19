using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Categories.UpdateActions;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class CategoryFixture : ClientFixture, IDisposable
    {
        private TypeFixture typeFixture;

        public CategoryFixture() : base()
        {
            this.CategoriesToDelete = new List<Category>();
            this.typeFixture = new TypeFixture();
        }

        public List<Category> CategoriesToDelete { get; private set; }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            // In case categories depend on each other, the order of deletion should be from the last to the first one.
            this.CategoriesToDelete.Reverse();
            foreach (Category category in this.CategoriesToDelete)
            {
                Category deletedCategory = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<Category>(new Guid(category.Id), category.Version)).Result;
            }

            this.typeFixture.Dispose();
        }

        public CategoryDraft GetCategoryDraft()
        {
            CategoryDraft categoryDraft = new CategoryDraft();
            string categoryName = TestingUtility.RandomString(10);
            LocalizedString localizedStringName = new LocalizedString();
            localizedStringName.Add("en", categoryName);
            categoryDraft.Name = localizedStringName;
            string slug = TestingUtility.RandomString(10);
            LocalizedString localizedStringSlug = new LocalizedString();
            localizedStringSlug.Add("en", slug);
            categoryDraft.Slug = localizedStringSlug;
            categoryDraft.Key = TestingUtility.RandomString(10);
            categoryDraft.OrderHint = TestingUtility.RandomSortOrder();
            return categoryDraft;
        }

        public Category CreateCategory()
        {
            return this.CreateCategory(this.GetCategoryDraft());
        }

        public Category CreateCategory(CategoryDraft categoryDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Category category = commerceToolsClient.ExecuteAsync(new CreateCommand<Category>(categoryDraft)).Result;
            return category;
        }

        public CategoryDraft GetCategoryDraftWithCustomFields()
        {
            Category relatedCategory = this.CreateCategory(this.GetCategoryDraft());
            this.CategoriesToDelete.Add(relatedCategory);
            CategoryDraft categoryDraft = this.GetCategoryDraft();
            CustomFieldsDraft customFieldsDraft = new CustomFieldsDraft();
            Type type = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(type);
            customFieldsDraft.Type = new ResourceIdentifier<Type> {Key = type.Key};
            customFieldsDraft.Fields = new Fields();
            customFieldsDraft.Fields.Add("string-field", "test");
            customFieldsDraft.Fields.Add("localized-string-field",
                new LocalizedString() {{"en", "localized-string-field-value"}});
            customFieldsDraft.Fields.Add("enum-field", "enum-key-1");
            customFieldsDraft.Fields.Add("localized-enum-field", "enum-key-1");
            customFieldsDraft.Fields.Add("number-field", 3);
            customFieldsDraft.Fields.Add("boolean-field", true);
            customFieldsDraft.Fields.Add("date-field", new DateTime(2018, 11, 28));
            customFieldsDraft.Fields.Add("date-time-field", new DateTime(2018, 11, 28, 11, 01, 00));
            customFieldsDraft.Fields.Add("time-field", new TimeSpan(11, 01, 00));
            customFieldsDraft.Fields.Add("money-field", new Money() {CentAmount = 1800, CurrencyCode = "EUR"});
            customFieldsDraft.Fields.Add("set-field", new FieldSet<string>() {"test1", "test2"});
            customFieldsDraft.Fields.Add("reference-field", new Reference<Category>() {Id = relatedCategory.Id});
            categoryDraft.Custom = customFieldsDraft;
            return categoryDraft;
        }

        public CategoryDraft GetCategoryDraftWithParent()
        {
            Category parentCategory = this.CreateCategory(this.GetCategoryDraft());
            this.CategoriesToDelete.Add(parentCategory);
            return this.GetCategoryDraftWithParent(parentCategory);
        }

        public CategoryDraft GetCategoryDraftWithParent(Category parentCategory)
        {
            CategoryDraft categoryDraft = this.GetCategoryDraft();
            categoryDraft.Parent = new Reference<Category>() {Id = parentCategory.Id};
            return categoryDraft;
        }

        /// <summary>
        /// Update Category set random Key and return updated instance
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Updated Category with newer version and updated random Key</returns>
        public Category UpdateCategorySetRandomKey(Category category)
        {
            IClient commerceToolsClient = this.GetService<IClient>();

            List<UpdateAction<Category>> updateActions = new List<UpdateAction<Category>>();
            SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction() {Key = TestingUtility.RandomString(10)};
            updateActions.Add(setKeyAction);
            Category updatedCategory = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Category>(new Guid(category.Id), category.Version, updateActions))
                .Result;
            return updatedCategory;
        }
    }
}
