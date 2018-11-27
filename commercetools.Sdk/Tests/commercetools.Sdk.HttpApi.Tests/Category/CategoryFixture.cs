using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.Tests
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
            this.CategoriesToDelete.Reverse();
            foreach (Category category in this.CategoriesToDelete)
            {
                Category deletedCategory = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Category>(new Guid(category.Id), category.Version)).Result;
            }
            this.typeFixture.Dispose();
        }

        public CategoryDraft GetCategoryDraft()
        {
            CategoryDraft categoryDraft = new CategoryDraft();
            string categoryName = this.RandomString(4);
            LocalizedString localizedStringName = new LocalizedString();
            localizedStringName.Add("en", categoryName);
            categoryDraft.Name = localizedStringName;
            string slug = this.RandomString(5);
            LocalizedString localizedStringSlug = new LocalizedString();
            localizedStringSlug.Add("en", slug);
            categoryDraft.Slug = localizedStringSlug;
            categoryDraft.Key = this.RandomString(3);
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
            customFieldsDraft.Type = new ResourceIdentifier() { Key = type.Key };
            customFieldsDraft.Fields = new Fields();
            customFieldsDraft.Fields.Add("string-field", "test");
            customFieldsDraft.Fields.Add("localized-string-field", new LocalizedString() { { "en", "localized-string-field-value" } });
            customFieldsDraft.Fields.Add("enum-field", "enum-key-1");
            customFieldsDraft.Fields.Add("localized-enum-field", "enum-key-1");
            customFieldsDraft.Fields.Add("number-field", 3);
            customFieldsDraft.Fields.Add("boolean-field", true);
            customFieldsDraft.Fields.Add("date-field", new DateTime(2018, 11, 28));
            customFieldsDraft.Fields.Add("date-time-field", new DateTime(2018, 11, 28, 11, 01, 00));
            customFieldsDraft.Fields.Add("time-field", new TimeSpan(11, 01, 00));
            customFieldsDraft.Fields.Add("money-field", new CentPrecisionMoney() { CentAmount = 1800, CurrencyCode = "EUR" });
            customFieldsDraft.Fields.Add("set-field", new Set<string>() { "test1", "test2" });
            customFieldsDraft.Fields.Add("reference-field", new Reference<Category>() { Id = relatedCategory.Id, TypeId = ReferenceTypeId.Category });
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
            categoryDraft.Parent = new Reference<Category>() { Id = parentCategory.Id, TypeId = ReferenceTypeId.Category };
            return categoryDraft;
        }
    }
}