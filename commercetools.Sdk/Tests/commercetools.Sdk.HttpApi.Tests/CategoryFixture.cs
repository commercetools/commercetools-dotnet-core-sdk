using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryFixture : ClientFixture, IDisposable
    {
        public CategoryFixture() : base()
        {
            this.CategoriesToDelete = new List<Category>();
            this.Client = this.GetService<IClient>();
            Category category = this.Client.ExecuteAsync(new CreateCommand<Category>(this.GetCategoryDraft())).Result;
            this.CreatedCategory = category;
            this.CategoriesToDelete.Add(category);
        }

        public List<Category> CategoriesToDelete { get; private set; }
        public IClient Client { get; private set; }
        public Category CreatedCategory { get; private set; }

        public void Dispose()
        {
            foreach (Category category in CategoriesToDelete)
            {
                Category deletedCategory = this.Client.ExecuteAsync(new DeleteByIdCommand<Category>(new Guid(category.Id), category.Version)).Result;
            }
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
    }
}