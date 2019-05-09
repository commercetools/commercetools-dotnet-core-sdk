using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.HttpApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain.Query;
using Xunit;
using commercetools.Sdk.Domain.Categories.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using ChangeNameUpdateAction = commercetools.Sdk.Domain.Categories.UpdateActions.ChangeNameUpdateAction;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    // All integration tests need to have the same collection name.
    [Collection("Integration Tests")]
    public class CategoryIntegrationTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly CategoryFixture categoryFixture;

        public CategoryIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.categoryFixture = new CategoryFixture(serviceProviderFixture);
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
        public async void DeleteCategoryByIdAndExpandParent()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent());

            //expansions
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> expand = new ReferenceExpansion<Category>(c => c.Parent);
            expansions.Add(expand);

            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Category>(new Guid(category.Id), category.Version, expansions)).Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<Category>(new Guid(retrievedCategory.Id))));

            Assert.NotNull(retrievedCategory.Parent.Obj);
            Assert.Equal(404, exception.StatusCode);
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
        public async void DeleteCategoryByKeyAndExpandParent()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent());

            //expansions
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> expand = new ReferenceExpansion<Category>(c => c.Parent);
            expansions.Add(expand);

            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new DeleteByKeyCommand<Category>(category.Key, category.Version, expansions)).Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<Category>(new Guid(retrievedCategory.Id))));

            Assert.NotNull(retrievedCategory.Parent.Obj);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void UpdateCategoryByIdSetKey()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            string newKey = TestingUtility.RandomString(10);
            List<UpdateAction<Category>> updateActions = new List<UpdateAction<Category>>();
            SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction() { Key = newKey };
            updateActions.Add(setKeyAction);
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<Category>(new Guid(category.Id), category.Version, updateActions)).Result;
            // The retrieved category has to be deleted and not the created category.
            // The retrieved category will have version 2 and the created category will have version 1.
            // Only the latest version can be deleted.
            this.categoryFixture.CategoriesToDelete.Add(retrievedCategory);
            Assert.Equal(newKey, retrievedCategory.Key);
        }

        [Fact]
        public void UpdateCategoryByIdSetKeyAndExpandParent()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent());
            string newKey = TestingUtility.RandomString(10);

            //expansions
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> expand = new ReferenceExpansion<Category>(c => c.Parent);
            expansions.Add(expand);

            //updateActions
            List<UpdateAction<Category>> updateActions = new List<UpdateAction<Category>>();
            SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction() { Key = newKey };
            updateActions.Add(setKeyAction);

            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<Category>(new Guid(category.Id), category.Version, updateActions, expansions)).Result;
            this.categoryFixture.CategoriesToDelete.Add(retrievedCategory);
            Assert.Equal(newKey, retrievedCategory.Key);
            Assert.NotNull(retrievedCategory.Parent.Obj);
        }


        [Fact]
        public void UpdateCategoryByKeySetExternalId()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            string externalId = TestingUtility.RandomString(10);
            List<UpdateAction<Category>> updateActions = new List<UpdateAction<Category>>();
            SetExternalIdUpdateAction setKeyAction = new SetExternalIdUpdateAction() { ExternalId = externalId };
            updateActions.Add(setKeyAction);
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new UpdateByKeyCommand<Category>(category.Key, category.Version, updateActions)).Result;
            this.categoryFixture.CategoriesToDelete.Add(retrievedCategory);
            Assert.Equal(externalId, retrievedCategory.ExternalId);
        }

        [Fact]
        public void UpdateCategoryByKeySetExternalIdAndExpandParent()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent());
            string externalId = TestingUtility.RandomString(10);

            //expansions
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> expand = new ReferenceExpansion<Category>(c => c.Parent);
            expansions.Add(expand);

            //updateActions
            List<UpdateAction<Category>> updateActions = new List<UpdateAction<Category>>();
            SetExternalIdUpdateAction setKeyAction = new SetExternalIdUpdateAction() { ExternalId = externalId };
            updateActions.Add(setKeyAction);

            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new UpdateByKeyCommand<Category>(category.Key, category.Version, updateActions, expansions)).Result;
            this.categoryFixture.CategoriesToDelete.Add(retrievedCategory);
            Assert.Equal(externalId, retrievedCategory.ExternalId);
            Assert.NotNull(retrievedCategory.Parent.Obj);
        }

        [Fact]
        public void UpdateCategoryByKeyChangeName()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            string name = TestingUtility.RandomString(10);
            List<UpdateAction<Category>> updateActions = new List<UpdateAction<Category>>();
            ChangeNameUpdateAction changeNameUpdateAction = new ChangeNameUpdateAction() { Name = new LocalizedString() { { "en", name } } };
            updateActions.Add(changeNameUpdateAction);
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new UpdateByKeyCommand<Category>(category.Key, category.Version, updateActions)).Result;
            this.categoryFixture.CategoriesToDelete.Add(retrievedCategory);
            Assert.Equal(name, retrievedCategory.Name["en"]);
        }

        [Fact]
        public void QueryCategory()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(category);
            string key = category.Key;
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Key == key);
            QueryCommand<Category> queryCommand = new QueryCommand<Category>();
            queryCommand.SetWhere(queryPredicate);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, c => c.Key == category.Key);
        }

        [Fact]
        public void QueryAndExpandParentCategory()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent());
            this.categoryFixture.CategoriesToDelete.Add(category);
            string key = category.Key;
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Key == key);
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> expand = new ReferenceExpansion<Category>(c => c.Parent);
            expansions.Add(expand);
            QueryCommand<Category> queryCommand = new QueryCommand<Category>();
            queryCommand.SetWhere(queryPredicate);
            queryCommand.SetExpand(expansions);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, c => c.Key == category.Key && c.Parent.Obj != null);
        }

        [Fact]
        public void QueryAndSortCategory()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category parentCategory = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(parentCategory);
            for (int i = 0; i < 3; i++)
            {
                Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent(parentCategory));
                this.categoryFixture.CategoriesToDelete.Add(category);
            }

            string id = parentCategory.Id;
            QueryCommand<Category> queryCommand = new QueryCommand<Category>();
            queryCommand.Sort(c => c.Name["en"]);
            queryCommand.Where(c => c.Parent.Id == id);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
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

            string id = parentCategory.Id;
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Parent.Id == id);
            List<Sort<Category>> sortPredicates = new List<Sort<Category>>();
            Sort<Category> sort = new Sort<Category>(c => c.Name["en"], SortDirection.Descending);
            sortPredicates.Add(sort);
            QueryCommand<Category> queryCommand = new QueryCommand<Category>();
            queryCommand.SetSort(sortPredicates);
            queryCommand.SetWhere(queryPredicate);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            var sortedList = returnedSet.Results.OrderByDescending(c => c.Name["en"]);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }

        [Fact]
        public void QueryAndLimitCategory()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category parentCategory = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(parentCategory);
            for (int i = 0; i < 3; i++)
            {
                Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent(parentCategory));
                this.categoryFixture.CategoriesToDelete.Add(category);
            }

            string id = parentCategory.Id;
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Parent.Id == id);
            QueryCommand<Category> queryCommand = new QueryCommand<Category>();
            queryCommand.SetWhere(queryPredicate);
            queryCommand.Limit = 2;
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Equal(2, returnedSet.Results.Count);
            Assert.Equal(3, returnedSet.Total);
        }

        [Fact]
        public void QueryAndOffsetCategory()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category parentCategory = this.categoryFixture.CreateCategory();
            this.categoryFixture.CategoriesToDelete.Add(parentCategory);
            for (int i = 0; i < 3; i++)
            {
                Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent(parentCategory));
                this.categoryFixture.CategoriesToDelete.Add(category);
            }

            string id = parentCategory.Id;
            QueryCommand<Category> queryCommand = new QueryCommand<Category>();
            queryCommand.Where(c => c.Parent.Id == id);
            queryCommand.Offset = 2;
            PagedQueryResult<Category> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Single(returnedSet.Results);
            Assert.Equal(3, returnedSet.Total);
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

        [Fact]
        public void GetCategoryByIdExpandParent()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent());
            this.categoryFixture.CategoriesToDelete.Add(category);
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(new Guid(category.Id)).Expand(c => c.Parent)).Result;
            Assert.NotNull(retrievedCategory.Parent);
            Assert.NotNull(retrievedCategory.Parent.Obj);
        }

        [Fact]
        public void GetCategoryByKeyExpandParent()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();
            Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent());
            this.categoryFixture.CategoriesToDelete.Add(category);
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> expand = new ReferenceExpansion<Category>(c => c.Parent);
            expansions.Add(expand);
            Category retrievedCategory = commerceToolsClient.ExecuteAsync(new GetByKeyCommand<Category>(category.Key, expansions)).Result;
            Assert.NotNull(retrievedCategory.Parent);
            Assert.NotNull(retrievedCategory.Parent.Obj);
        }

        [Fact]
        public void QueryContextLinqProvider()
        {
            IClient commerceToolsClient = this.categoryFixture.GetService<IClient>();

            Category category = this.categoryFixture.CreateCategory(this.categoryFixture.GetCategoryDraftWithParent());
            this.categoryFixture.CategoriesToDelete.Add(category);

            var query = from c in commerceToolsClient.Query<Category>()
                where c.Key == category.Key.valueOf()
                orderby c.Key descending
                select c;

            query.Expand(c => c.Parent).Expand(c => c.Ancestors.ExpandAll());

            var command = ((ClientQueryProvider<Category>) query.Provider).Command;
            Assert.Equal($"key = \"{category.Key}\"", string.Join(", ",command.Where));
            Assert.Equal("key desc", string.Join(", ", command.Sort));
            Assert.Equal("parent, ancestors[*]", string.Join(", ", command.Expand));

            var categories = query.ToList();
            Assert.Single(categories);
            Assert.Equal(category.Key, categories.First().Key);
            Assert.Equal(category.Parent.Id, categories.First().Parent.Obj.Id);
            Assert.Equal(category.Parent.Id, categories.First().Ancestors.First().Id);
        }
    }
}
