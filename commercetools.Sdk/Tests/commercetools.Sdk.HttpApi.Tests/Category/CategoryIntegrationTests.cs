using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryIntegrationTests : IClassFixture<CategoryFixture>
    {
        private readonly CategoryFixture categoryFixture;

        public CategoryIntegrationTests(CategoryFixture categoryFixture)
        {
            this.categoryFixture = categoryFixture;
        }

        [Fact]
        public void GetCategoryById()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(category);
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(new Guid(category.Id))).Result;
            Assert.Equal(category.Id, retrievedCategory.Id);
        }

        [Fact]
        public void GetCategoryByKey()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(category);
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new GetByKeyCommand<Category>(category.Key)).Result;
            Assert.Equal(category.Key, retrievedCategory.Key);
        }

        [Fact]
        public void CreateCategory()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            CategoryDraft categoryDraft = this.categoryFixture.GetCategoryDraft();
            Category category = commerceToolsClient.ExecuteAsync(new CreateCommand<Category>(categoryDraft)).Result;
            this.categoryFixture.CategoriesToDelete.Add(category);
            Assert.Equal(categoryDraft.Key, category.Key.ToString());
        }

        [Fact]
        public void DeleteCategoryById()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Category>(new Guid(category.Id), category.Version)).Result;
            Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(new Guid(retrievedCategory.Id))));
        }

        [Fact]
        public void DeleteCategoryByKey()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new DeleteByKeyCommand<Category>(category.Key, category.Version)).Result;
            Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.ExecuteAsync<Category>(new GetByIdCommand<Category>(new Guid(retrievedCategory.Id))));
        }

        [Fact]
        public void UpdateCategoryByIdSetKey()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            string newKey = this.categoryFixture.RandomString(5);
            List<UpdateAction<Category>> updateActions = new List<UpdateAction<Category>>();
            SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction() { Key = newKey };
            updateActions.Add(setKeyAction);
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<Category>(new Guid(category.Id), category.Version, updateActions)).Result;
            this.categoryFixture.CategoriesToDelete.Add(retrievedCategory);
            Assert.Equal(newKey, retrievedCategory.Key);
        }

        [Fact]
        public void UpdateCategoryByKeySetExternalId()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            string externalId = this.categoryFixture.RandomString(5);
            List<UpdateAction<Category>> updateActions = new List<UpdateAction<Category>>();
            SetExternalIdUpdateAction setKeyAction = new SetExternalIdUpdateAction() { ExternalId = externalId };
            updateActions.Add(setKeyAction);
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new UpdateByKeyCommand<Category>(category.Key, category.Version, updateActions)).Result;
            this.categoryFixture.CategoriesToDelete.Add(retrievedCategory);
            Assert.Equal(externalId, retrievedCategory.ExternalId);
        }

        [Fact]
        public void QueryCategory()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(category);
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Key == category.Key);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(new QueryCommand<Category>() { QueryPredicate = queryPredicate }).Result;
            Assert.Contains(returnedSet.Results, c => c.Key == category.Key); 
        }

        [Fact]
        public void QueryAndExpandParentCategory()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent());
            this.categoryFixture.CategoriesToDelete.Add(category);
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Key == category.Key);
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> expand = new ReferenceExpansion<Category>(c => c.Parent);
            expansions.Add(expand);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(new QueryCommand<Category>() { QueryPredicate = queryPredicate, Expand = expansions }).Result;
            Assert.Contains(returnedSet.Results, c => c.Key == category.Key && c.Parent.Obj != null);
        }

        [Fact]
        public void QueryAndSortCategory()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category parentCategory = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(parentCategory);
            for(int i = 0; i < 3; i++)
            {
                Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent(parentCategory));
                this.categoryFixture.CategoriesToDelete.Add(category);
            }
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Parent.Id == parentCategory.Id);
            List<Sort<Category>> sortPredicates = new List<Sort<Category>>();
            Sort<Category> sort = new Sort<Category>(c => c.Name["en"]);
            sortPredicates.Add(sort);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(new QueryCommand<Category>() { QueryPredicate = queryPredicate, Sort = sortPredicates }).Result;
            var sortedList = returnedSet.Results.OrderBy(c => c.Name["en"]);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }

        [Fact]
        public void QueryAndSortCategoryDescending()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category parentCategory = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(parentCategory);
            for (int i = 0; i < 3; i++)
            {
                Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent(parentCategory));
                this.categoryFixture.CategoriesToDelete.Add(category);
            }
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Parent.Id == parentCategory.Id);
            List<Sort<Category>> sortPredicates = new List<Sort<Category>>();
            Sort<Category> sort = new Sort<Category>(c => c.Name["en"], SortDirection.Descending);
            sortPredicates.Add(sort);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(new QueryCommand<Category>() { QueryPredicate = queryPredicate, Sort = sortPredicates }).Result;
            var sortedList = returnedSet.Results.OrderByDescending(c => c.Name["en"]);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }

        [Fact]
        public void CreateCategoryWithCustomFields()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            CategoryDraft categoryDraft = this.categoryFixture.GetCategoryDraftWithCustomFields();
            Category category = this.categoryFixture.CreateCategory(categoryDraft);            
            this.categoryFixture.CategoriesToDelete.Add(category);
            Assert.Equal(categoryDraft.Custom.Fields.Count, category.Custom.Fields.Count);
        }
    }
}
