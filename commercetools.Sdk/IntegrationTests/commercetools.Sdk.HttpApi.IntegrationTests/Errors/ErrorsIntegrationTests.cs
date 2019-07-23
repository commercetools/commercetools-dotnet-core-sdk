using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class ErrorsIntegrationTests : IDisposable
    {
        private readonly ErrorsFixture errorsFixture;

        public ErrorsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.errorsFixture = new ErrorsFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.errorsFixture.Dispose();
        }

        [Fact]
        public async void CheckNotFoundException()
        {
            IClient commerceToolsClient = this.errorsFixture.GetService<IClient>();
            Guid categoryId = Guid.NewGuid(); //not exists
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(categoryId)));
            Assert.Equal(404, exception.StatusCode);
            //var category = await commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(categoryId));
        }

        [Fact]
        public async void CheckErrorResponseException()
        {
            IClient commerceToolsClient = this.errorsFixture.GetService<IClient>();
            string productTypeId = Guid.NewGuid().ToString(); //references another resource that does not exist
            ProductDraft productDraft = new ProductDraft();
            productDraft.Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};
            productDraft.Slug = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};
            productDraft.ProductType = new ResourceIdentifier<ProductType> {Id = productTypeId};
            ErrorResponseException exception = await Assert.ThrowsAsync<ErrorResponseException>(() =>
                commerceToolsClient.ExecuteAsync(new CreateCommand<Product>(productDraft)));
            Assert.Equal(400, exception.StatusCode);
        }

        /// <summary>
        /// The request attempts to modify a resource that is out of date, i.e. that has been modified by another client since the last time it was retrieved.
        /// </summary>
        [Fact]
        public void CheckConcurrentModificationException()
        {
            ConcurrentModificationException concurrentModificationException = null;

            Category originalCategory = this.errorsFixture.CategoryFixture.CreateCategory();
            //update it, then it has a newer version
            Category updatedCategory = this.errorsFixture.CategoryFixture.UpdateCategorySetRandomKey(originalCategory);
            // then if we try to update the original category it will throw ConCurrentModification Exception
            try
            {
                this.errorsFixture.CategoryFixture.UpdateCategorySetRandomKey(originalCategory);
            }
            catch (Exception ex)
            {
                if (ex is AggregateException && ex.InnerException is ConcurrentModificationException exception)
                    concurrentModificationException = exception;
            }

            this.errorsFixture.CategoryFixture.CategoriesToDelete.Add(updatedCategory);
            Assert.NotNull(concurrentModificationException);
            Assert.Single(concurrentModificationException.ErrorResponse.Errors);
            Assert.IsType<ConcurrentModificationError>(concurrentModificationException.ErrorResponse.Errors[0]);
            Assert.Equal(concurrentModificationException.GetCurrentVersion(), updatedCategory.Version);
        }
    }
}
