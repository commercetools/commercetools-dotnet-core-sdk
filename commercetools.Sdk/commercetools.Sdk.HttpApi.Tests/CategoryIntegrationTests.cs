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
    public class CategoryIntegrationTests
    {
        [Fact]
        public void GetCategoryById()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            string categoryId = "2bafc816-4223-4ff0-ac8a-0f08a8f29fd6";
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand<Category>(new Guid(categoryId))).Result;
            Assert.Equal(categoryId, category.Id.ToString());
        }

        [Fact]
        public void GetCategoryByKey()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            string categoryKey = "c2";
            Category category = commerceToolsClient.Execute<Category>(new GetByKeyCommand<Category>(categoryKey)).Result;
            Assert.Equal(categoryKey, category.Key.ToString());
        }

        [Fact]
        public void CreateAndDeleteByIdCategory()
        {
            // create and delete are in the same method so that the repository does not get filled up with test categories
            IClient commerceToolsClient = TestUtils.SetupClient();
            CategoryDraft categoryDraft = new CategoryDraft();
            string categoryName = TestUtils.RandomString(4);
            LocalizedString localizedStringName = new LocalizedString();
            localizedStringName.Add("en", categoryName);
            categoryDraft.Name = localizedStringName;
            string slug = TestUtils.RandomString(5);
            LocalizedString localizedStringSlug = new LocalizedString();
            localizedStringSlug.Add("en", slug);
            categoryDraft.Slug = localizedStringSlug;
            Category category = commerceToolsClient.Execute<Category>(new CreateCommand<Category>(categoryDraft)).Result;
            Assert.Equal(categoryName, category.Name["en"]);
            Category deletedCategory = commerceToolsClient.Execute<Category>(new DeleteByIdCommand<Category>(new Guid(category.Id), category.Version)).Result;
            Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.Execute<Category>(new GetByIdCommand<Category>(new Guid(deletedCategory.Id))));
        }

        [Fact]
        public void CreateAndDeleteByKeyCategory()
        {
            // create and delete are in the same method so that the repository does not get filled up with test categories
            IClient commerceToolsClient = TestUtils.SetupClient();
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
            Category category = commerceToolsClient.Execute<Category>(new CreateCommand<Category>(categoryDraft)).Result;
            Assert.Equal(categoryName, category.Name["en"]);
            Category deletedCategory = commerceToolsClient.Execute<Category>(new DeleteByKeyCommand<Category>(category.Key, category.Version)).Result;
            Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.Execute<Category>(new GetByIdCommand<Category>(new Guid(deletedCategory.Id))));
        }

        [Fact]
        public void UpdateCategoryById()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            string categoryId = "8994e5d7-d81f-4480-af60-286dc96c1fe8";
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand<Category>(new Guid(categoryId))).Result;
            string currentKey = category.Key;
            SetKey setKeyAction = new SetKey();
            setKeyAction.Key = "newKey" + TestUtils.RandomString(3);
            Category updatedCategory = commerceToolsClient.Execute<Category>(new UpdateByIdCommand<Category>(new Guid(category.Id), category.Version, new List<UpdateAction>() { setKeyAction })).Result;
            Assert.Equal(updatedCategory.Key, setKeyAction.Key);
        }

        [Fact]
        public void UpdateCategoryByKey()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            string categoryId = "8994e5d7-d81f-4480-af60-286dc96c1fe8";
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand<Category>(new Guid(categoryId))).Result;
            ChangeOrderHint changeOrderHint = new ChangeOrderHint();
            changeOrderHint.OrderHint = TestUtils.RandomString(6);
            SetExternalId setExternalId = new SetExternalId();
            setExternalId.ExternalId = TestUtils.RandomString(5);
            Category updatedCategory = commerceToolsClient.Execute<Category>(new UpdateByKeyCommand<Category>(category.Key, category.Version, new List<UpdateAction>() { changeOrderHint, setExternalId })).Result;
            Assert.Equal(updatedCategory.OrderHint, changeOrderHint.OrderHint);
            Assert.Equal(updatedCategory.ExternalId, setExternalId.ExternalId);
        }

        [Fact]
        public void QueryCategory()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Key == "c14");
            PagedQueryResult<Category> returnedSet = commerceToolsClient.Execute(new QueryCommand<Category>() { QueryPredicate = queryPredicate }).Result;
            Assert.Contains(returnedSet.Results, c => c.Key == "c14"); 
        }

        [Fact]
        public void QueryAndExpandParentCategory()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Key == "c22");
            List<Expansion<Category>> expandList = new List<Expansion<Category>>();
            ReferenceExpansion<Category> expand = new ReferenceExpansion<Category>(c => c.Parent);
            expandList.Add(expand);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.Execute(new QueryCommand<Category>() { QueryPredicate = queryPredicate, Expand = expandList }).Result;
            Assert.Contains(returnedSet.Results, c => c.Key == "c22" && c.Parent.Obj != null);
        }

        [Fact]
        public void QueryAndSortCategory()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Parent.Id == "f40fcd15-b1c2-4279-9cfa-f6083e6a2988");
            List<Sort<Category>> sortList = new List<Sort<Category>>();
            Sort<Category> sort = new Sort<Category>(c => c.Name["en"]);
            sortList.Add(sort);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.Execute(new QueryCommand<Category>() { QueryPredicate = queryPredicate, Sort = sortList }).Result;
            var sortedList = returnedSet.Results.OrderBy(c => c.Name["en"]);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }

        [Fact]
        public void QueryAndSortCategoryDescending()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Parent.Id == "f40fcd15-b1c2-4279-9cfa-f6083e6a2988");
            List<Sort<Category>> sortList = new List<Sort<Category>>();
            Sort<Category> sort = new Sort<Category>(c => c.Name["en"], SortDirection.Descending);
            sortList.Add(sort);
            PagedQueryResult<Category> returnedSet = commerceToolsClient.Execute(new QueryCommand<Category>() { QueryPredicate = queryPredicate, Sort = sortList }).Result;
            var sortedList = returnedSet.Results.OrderByDescending(c => c.Name["en"]);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }
    }
}
