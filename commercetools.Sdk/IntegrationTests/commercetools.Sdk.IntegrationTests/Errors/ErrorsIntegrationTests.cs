using System;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Categories.UpdateActions;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Categories.CategoriesFixture;

namespace commercetools.Sdk.IntegrationTests.Errors
{
    [Collection("Integration Tests")]
    public class ErrorsIntegrationTests
    {
        private readonly IClient client;

        public ErrorsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async void CheckNotFoundException()
        {
            var categoryId = Guid.NewGuid(); //not exists
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                client.ExecuteAsync(new GetByIdCommand<Category>(categoryId)));
            Assert.NotNull(exception);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public async void CheckErrorResponseException()
        {
            string productTypeId = Guid.NewGuid().ToString(); //references another resource that does not exist
            var productDraft = new ProductDraft
            {
                Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}},
                Slug = new LocalizedString() {{"en", TestingUtility.RandomString(10)}},
                ProductType = new ResourceIdentifier<ProductType> {Id = productTypeId}
            };
            var exception = await Assert.ThrowsAsync<ErrorResponseException>(() =>
                client.ExecuteAsync(new CreateCommand<Product>(productDraft)));
            Assert.Equal(400, exception.StatusCode);
        }

        /// <summary>
        /// The request attempts to modify a resource that is out of date, i.e. that has been modified by another client since the last time it was retrieved.
        /// </summary>
        [Fact]
        public async void CheckConcurrentModificationException()
        {
            await WithUpdateableCategory(client, async category =>
                {
                    var key = TestingUtility.RandomString();
                    var action = new SetKeyUpdateAction {Key = key};

                    var updatedCategory = await client
                        .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(key, updatedCategory.Key);

                    // updatedCategory now has a new version
                    // then if we try to update the original category it will throw ConCurrentModification Exception

                    var exception = await Assert.ThrowsAsync<ConcurrentModificationException>(() =>
                        client.ExecuteAsync(
                            category.UpdateById(actions => actions.AddUpdate(action))));

                    Assert.NotNull(exception);
                    Assert.Single(exception.ErrorResponse.Errors);
                    Assert.IsType<ConcurrentModificationError>(exception.ErrorResponse.Errors[0]);
                    Assert.Equal(exception.GetCurrentVersion(), updatedCategory.Version);
                    return updatedCategory;
                });
        }
    }
}