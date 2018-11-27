using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.HttpApi.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ExceptionTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public ExceptionTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void GetCategoryByIdThrowsNotFoundException()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            string categoryId = "2bafc816-4223-4ff0-ac8a-0f08a8f29fd7";
            HttpApiClientException exception = Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.ExecuteAsync<Category>(new GetByIdCommand<Category>(new Guid(categoryId)))).Result;
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void UpdateCategoryByIdThrowsConcurrentModificationException()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            string categoryId = "8994e5d7-d81f-4480-af60-286dc96c1fe8";
            Category category = commerceToolsClient.ExecuteAsync<Category>(new GetByIdCommand<Category>(new Guid(categoryId))).Result;
            string currentKey = category.Key;
            SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction();
            setKeyAction.Key = "newKey" + this.clientFixture.RandomString(3);
            HttpApiClientException exception = Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.ExecuteAsync<Category>(new UpdateByIdCommand<Category>(new Guid(category.Id), category.Version - 1, new List<UpdateAction<Category>>() { setKeyAction }))).Result;
            Assert.Equal(409, exception.StatusCode);
            Assert.Equal(typeof(ConcurrentModificationError), exception.Errors[0].GetType());
        }
    }
}
