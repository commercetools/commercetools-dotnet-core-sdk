using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using Moq;
using System;
using System.Net.Http;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryRequestMessageTests
    {
        [Fact]
        public void CategoryGetByIdRequestMessage()
        {
            GetByIdCommand<Category> command = new GetByIdCommand<Category>(new Guid("2bafc816-4223-4ff0-ac8a-0f08a8f29fd6"));
            var clientConfiguration = new Mock<IClientConfiguration>();
            clientConfiguration.Setup(x => x.ApiBaseAddress).Returns("https://api.sphere.io/portablevendor");
            GetRequestMessageBuilder requestMessageBuilder = new GetRequestMessageBuilder(clientConfiguration.Object);
            HttpRequestMessage httpRequestMessage = requestMessageBuilder.GetRequestMessage(command);
            Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
            Assert.Equal("https://api.sphere.io/portablevendor/categories/2bafc816-4223-4ff0-ac8a-0f08a8f29fd6", httpRequestMessage.RequestUri.ToString());
        }
    }
}
