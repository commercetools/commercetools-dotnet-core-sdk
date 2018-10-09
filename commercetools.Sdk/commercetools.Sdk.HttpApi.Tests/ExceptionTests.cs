using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ExceptionTests
    {
        [Fact]
        public void GetCategoryByIdThrowsNotFoundException()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            string categoryId = "2bafc816-4223-4ff0-ac8a-0f08a8f29fd7";
            HttpApiClientException exception = Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.Execute<Category>(new GetByIdCommand<Category>(new Guid(categoryId)))).Result;
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void UpdateCategoryByIdThrowsConcurrentModificationException()
        {
            IClient commerceToolsClient = TestUtils.SetupClient();
            string categoryId = "8994e5d7-d81f-4480-af60-286dc96c1fe8";
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand<Category>(new Guid(categoryId))).Result;
            string currentKey = category.Key;
            SetKey setKeyAction = new SetKey();
            setKeyAction.Key = "newKey" + TestUtils.RandomString(3);
            HttpApiClientException exception = Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.Execute<Category>(new UpdateByIdCommand<Category>(new Guid(category.Id), category.Version - 1, new List<UpdateAction>() { setKeyAction }))).Result;
            Assert.Equal(409, exception.StatusCode);
            Assert.Equal(typeof(ConcurrentModificationError), exception.Errors[0].GetType());
        }
    }
}
