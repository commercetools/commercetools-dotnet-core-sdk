using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Serialization;
using System;
using System.Collections.Generic;
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
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand(new Guid(categoryId))).Result;
            Assert.Equal(categoryId, category.Id.ToString());
        }

        [Fact]
        public void GetCategoryByKey()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            string categoryKey = "c2";
            Category category = commerceToolsClient.Execute<Category>(new GetByKeyCommand(categoryKey)).Result;
            Assert.Equal(categoryKey, category.Key.ToString());
        }

        [Fact]
        public void CreateAndDeleteCategory()
        {
            // create and delete are in the same method so that the client does not get field up with test categories
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
            Category category = commerceToolsClient.Execute<Category>(new CreateCommand(categoryDraft)).Result;
            Assert.Equal(categoryName, category.Name["en"]);
            Category deletedCategory = commerceToolsClient.Execute<Category>(new DeleteByIdCommand(new Guid(category.Id), category.Version)).Result;
            Assert.ThrowsAsync<ResourceNotFoundException>(() => commerceToolsClient.Execute<Category>(new GetByIdCommand(new Guid(deletedCategory.Id))));
        }

        [Fact]
        public void UpdateCategoryById()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            string categoryId = "8994e5d7-d81f-4480-af60-286dc96c1fe8";
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand(new Guid(categoryId))).Result;
            string currentKey = category.Key;
            SetKey setKeyAction = new SetKey();
            setKeyAction.Key = "newKey" + TestUtils.RandomString(3);
            Category updatedCategory = commerceToolsClient.Execute<Category>(new UpdateByIdCommand(new Guid(category.Id), category.Version, new List<UpdateAction>() { setKeyAction })).Result;
            Assert.Equal(updatedCategory.Key, setKeyAction.Key);
        }

        [Fact]
        public void UpdateCategoryByKey()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            string categoryId = "8994e5d7-d81f-4480-af60-286dc96c1fe8";
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand(new Guid(categoryId))).Result;
            ChangeOrderHint changeOrderHint = new ChangeOrderHint();
            changeOrderHint.OrderHint = TestUtils.RandomString(6);
            SetExternalId setExternalId = new SetExternalId();
            setExternalId.ExternalId = TestUtils.RandomString(5);
            Category updatedCategory = commerceToolsClient.Execute<Category>(new UpdateByKeyCommand(category.Key, category.Version, new List<UpdateAction>() { changeOrderHint, setExternalId })).Result;
            Assert.Equal(updatedCategory.OrderHint, changeOrderHint.OrderHint);
            Assert.Equal(updatedCategory.ExternalId, setExternalId.ExternalId);
        }
    }
}
