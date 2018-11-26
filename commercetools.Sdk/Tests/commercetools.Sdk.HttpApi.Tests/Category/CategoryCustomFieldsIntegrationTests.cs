using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryCustomFieldsIntegrationTests : IClassFixture<CategoryFixture>
    {
        private readonly CategoryFixture categoryFixture;

        public CategoryCustomFieldsIntegrationTests(CategoryFixture categoryFixture)
        {
            this.categoryFixture = categoryFixture;
        }

        [Fact]
        public void CreateCategoryWithCustomFields()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            CategoryDraft categoryDraft = this.categoryFixture.GetCategoryDraftWithCustomFields();
            Category createdCategory = commerceToolsClient.ExecuteAsync(new CreateCommand<Category>(categoryDraft)).Result;
            this.categoryFixture.CategoriesToDelete.Add(createdCategory);
            Assert.Equal(categoryDraft.Custom.Fields.Count, createdCategory.Custom.Fields.Count);
        }
    }
}
