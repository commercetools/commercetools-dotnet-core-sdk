using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryFixture : ClientFixture, IDisposable
    {
        private List<Category> categoriesToDelete = new List<Category>();

        public CategoryFixture() : base()
        {
            this.Client = this.GetService<IClient>();
            Category category = this.Client.ExecuteAsync(new CreateCommand<Category>(this.GetCategoryDraft())).Result;
            this.CreatedCategory = category;
            this.categoriesToDelete.Add(category);
        }

        public IClient Client { get; private set; }
        public Category CreatedCategory { get; private set; }

        public void Dispose()
        {
            foreach (Category category in categoriesToDelete)
            {
                Category deletedCategory = this.Client.ExecuteAsync(new DeleteByIdCommand<Category>(new Guid(category.Id), category.Version)).Result;
            }
        }

        public CategoryDraft GetCategoryDraft()
        {
            CategoryDraft categoryDraft = new CategoryDraft();
            string categoryName = TestUtils.RandomString(4);
            LocalizedString localizedStringName = new LocalizedString();
            localizedStringName.Add("en", categoryName);
            categoryDraft.Name = localizedStringName;
            string slug = TestUtils.RandomString(5);
            LocalizedString localizedStringSlug = new LocalizedString();
            localizedStringSlug.Add("en", slug);
            categoryDraft.Slug = localizedStringSlug;
            categoryDraft.Key = TestUtils.RandomString(3);
            return categoryDraft;
        }
    }
}