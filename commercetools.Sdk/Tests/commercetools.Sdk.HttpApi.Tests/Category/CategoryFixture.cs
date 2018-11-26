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
            this.Client = this.GetService<IClient>();
            Category category = this.Client.ExecuteAsync(new CreateCommand<Category>(this.GetCategoryDraft())).Result;
            this.CreatedCategory = category;
            this.CategoriesToDelete.Add(category);
            this.typeFixture = new TypeFixture();
        }

        public List<Category> CategoriesToDelete { get; private set; }
        public IClient Client { get; private set; }
        public Category CreatedCategory { get; private set; }

        public void Dispose()
        {           
            foreach (Category category in this.CategoriesToDelete)
            {
                Category deletedCategory = this.Client.ExecuteAsync(new DeleteByIdCommand<Category>(new Guid(category.Id), category.Version)).Result;
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
            Category category = this.Client.ExecuteAsync(new CreateCommand<Category>(this.GetCategoryDraft())).Result;
            this.CategoriesToDelete.Add(category);
            return category;
        }

        public CategoryDraft GetCategoryDraftWithCustomFields()
        {
            Category relatedCategory = this.CreateCategory();
            CategoryDraft categoryDraft = this.GetCategoryDraft();
            CustomFieldsDraft customFieldsDraft = new CustomFieldsDraft();
            Type type = this.typeFixture.CreateType();
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
            customFieldsDraft.Fields.Add("reference-field", new Reference<Category>() { Id = relatedCategory.Id, TypeId = "category" });
            categoryDraft.Custom = customFieldsDraft; 
            return categoryDraft;
        }
    }
}