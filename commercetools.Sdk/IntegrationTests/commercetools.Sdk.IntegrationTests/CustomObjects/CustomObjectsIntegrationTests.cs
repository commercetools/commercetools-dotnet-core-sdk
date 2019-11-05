using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomObject;
using commercetools.Sdk.HttpApi.Extensions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.CustomObjects.CustomObjectsFixture;

namespace commercetools.Sdk.IntegrationTests.CustomObjects
{
    [Collection("Integration Tests")]
    public class CustomObjectsIntegrationTests
    {
        private readonly IClient client;

        public CustomObjectsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateCustomObjectAsFooBar()
        {
            var key = $"FooBar-{TestingUtility.RandomString()}";
            var fooBar = new FooBar();
            await WithCustomObject<FooBar>(
                client,
                customObjectDraft => DefaultCustomObjectDraftWithKey(customObjectDraft, fooBar, key),
                fooBarCustomObject =>
                {
                    Assert.Equal(key, fooBarCustomObject.Key);
                    Assert.Equal(fooBar.Bar, fooBarCustomObject.Value.Bar);
                });
        }

        [Fact]
        public async Task CreateCustomObjectAsString()
        {
            var key = $"FooBar-{TestingUtility.RandomString()}";
            var value = TestingUtility.RandomString();
            await WithCustomObject<string>(
                client,
                customObjectDraft => DefaultCustomObjectDraftWithKey(customObjectDraft, value, key),
                fooBarCustomObject =>
                {
                    Assert.Equal(key, fooBarCustomObject.Key);
                    Assert.Equal(value, fooBarCustomObject.Value);
                });
        }

        [Fact]
        public async Task GetCustomObjectById()
        {
            var key = $"FooBar-{TestingUtility.RandomString()}";
            var fooBar = new FooBar();
            await WithCustomObject<FooBar>(
                client,
                customObjectDraft => DefaultCustomObjectDraftWithKey(customObjectDraft, fooBar, key),
                async customObject =>
                {
                    var retrievedCustomObject = await client
                        .ExecuteAsync(new GetByIdCommand<CustomObject>(customObject.Id));
                    Assert.Equal(key, retrievedCustomObject.Key);
                    var fooBarCustomObject = retrievedCustomObject.GetValueAs<FooBar>();

                    Assert.NotNull(fooBarCustomObject);
                    Assert.Equal(customObject.Value.Bar, fooBarCustomObject.Bar);
                });
        }
        
        [Fact]
        public async Task GetCustomObjectByContainerAndKey()
        {
            var fooBar = new FooBar();
            await WithCustomObject<FooBar>(
                client,
                customObjectDraft => DefaultCustomObjectDraft(customObjectDraft, fooBar),
                async customObject =>
                {
                    var retrievedCustomObject = await client
                        .ExecuteAsync(new GetByContainerAndKeyCommand<CustomObject>(customObject.Container, customObject.Key));
                    Assert.Equal(customObject.Container, retrievedCustomObject.Container);
                    Assert.Equal(customObject.Key, retrievedCustomObject.Key);

                    var fooBarCustomObject = retrievedCustomObject.GetValueAs<FooBar>();

                    Assert.NotNull(fooBarCustomObject);
                    Assert.Equal(customObject.Value.Bar, fooBarCustomObject.Bar);
                });
        }
    }
}