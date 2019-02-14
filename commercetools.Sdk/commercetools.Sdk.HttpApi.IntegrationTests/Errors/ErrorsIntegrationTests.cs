using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Errors
{
    [Collection("Integration Tests")]
    public class ErrorsIntegrationTests : IClassFixture<ErrorsFixture>
    {
        private readonly ErrorsFixture errorsFixture;

        public ErrorsIntegrationTests(ErrorsFixture errorsFixture)
        {
            this.errorsFixture = errorsFixture;
        }
        [Fact]
        public async void NotFoundException()
        {
            IClient commerceToolsClient = this.errorsFixture.GetService<IClient>();
            Guid categoryId = Guid.NewGuid();//not exists
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(()=> commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(categoryId)));
            Assert.Equal(404, exception.StatusCode);
        }
        [Fact]
        public async void ErrorResponseException()
        {
            IClient commerceToolsClient = this.errorsFixture.GetService<IClient>();
            string productTypeId = Guid.NewGuid().ToString();//references another resource that does not exist
            ProductDraft productDraft = new ProductDraft();
            productDraft.Name = new LocalizedString() {{"en", this.errorsFixture.RandomString(4)}};
            productDraft.Slug = new LocalizedString() {{"en", this.errorsFixture.RandomString(3)}};
            productDraft.ProductType = new ResourceIdentifier() {Id = productTypeId};
            ErrorResponseException exception = await Assert.ThrowsAsync<ErrorResponseException>(()=> commerceToolsClient.ExecuteAsync(new CreateCommand<Product>(productDraft)));
            Assert.Equal(400, exception.StatusCode);
        }
    }
}