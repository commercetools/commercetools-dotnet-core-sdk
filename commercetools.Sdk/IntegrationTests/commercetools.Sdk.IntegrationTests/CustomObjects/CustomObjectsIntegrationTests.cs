using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomObject;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
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
                        .ExecuteAsync(
                            new GetByContainerAndKeyCommand<CustomObject>(customObject.Container, customObject.Key));
                    Assert.Equal(customObject.Container, retrievedCustomObject.Container);
                    Assert.Equal(customObject.Key, retrievedCustomObject.Key);

                    var fooBarCustomObject = retrievedCustomObject.GetValueAs<FooBar>();

                    Assert.NotNull(fooBarCustomObject);
                    Assert.Equal(customObject.Value.Bar, fooBarCustomObject.Bar);
                });
        }

        [Fact]
        public async Task UpdateCustomObject()
        {
            var key = $"FooBar-{TestingUtility.RandomString()}";
            var fooBar = new FooBar();
            await WithUpdateableCustomObjectAsync<FooBar>(
                client,
                customObjectDraft => DefaultCustomObjectDraftWithKey(customObjectDraft, fooBar, key),
                async (customObject, customObjectDraft) =>
                {
                    Assert.Equal(key, customObject.Key);

                    //Then Update Custom Object Value
                    customObjectDraft.Value.Bar = TestingUtility.RandomString(10);

                    var updatedCustomObject = await client
                        .ExecuteAsync(new CustomObjectUpsertCommand<FooBar>(customObjectDraft));

                    Assert.Equal(customObjectDraft.Key, updatedCustomObject.Key);
                    Assert.Equal(customObjectDraft.Value.Bar, updatedCustomObject.Value.Bar);
                    return updatedCustomObject;
                });
        }

        [Fact]
        public async Task DeleteCustomObjectById()
        {
            var key = $"FooBar-{TestingUtility.RandomString()}";
            var fooBar = new FooBar();
            await WithCustomObject<FooBar>(
                client,
                customObjectDraft => DefaultCustomObjectDraftWithKey(customObjectDraft, fooBar, key),
                async customObject =>
                {
                    Assert.Equal(key, customObject.Key);
                    await client.ExecuteAsync(
                        new DeleteByIdCommand<CustomObject>(customObject.Id, customObject.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<CustomObject>(customObject.Id))
                    );
                });
        }

        [Fact]
        public async Task DeleteCustomObjectByContainerAndKey()
        {
            var key = $"FooBar-{TestingUtility.RandomString()}";
            var fooBar = new FooBar();
            await WithCustomObject<FooBar>(
                client,
                customObjectDraft => DefaultCustomObjectDraftWithKey(customObjectDraft, fooBar, key),
                async customObject =>
                {
                    Assert.Equal(key, customObject.Key);
                    await client.ExecuteAsync(
                        new DeleteByContainerAndKeyCommand<CustomObject>(customObject.Container, customObject.Key));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<CustomObject>(customObject.Id))
                    );
                });
        }

        [Fact]
        public async Task QueryCustomObjectsByContainer()
        {
            //Create container with 5 Custom FooBar Objects and container with 3 Foo Objects
            string fooBarContainer = $"FooBarContainer{TestingUtility.RandomInt()}";
            string fooContainer = $"FooContainer{TestingUtility.RandomInt()}";

            await WithListOfCustomObject<FooBar>(client,
                customObjectDraft => DefaultCustomObjectDraftWithContainerName(customObjectDraft, fooBarContainer),
                5, async fooBarList =>
                {
                    Assert.Equal(5, fooBarList.Count);
                    await WithListOfCustomObject<Foo>(client,
                        customObjectDraft => DefaultCustomObjectDraftWithContainerName(customObjectDraft, fooContainer),
                        3, async fooList =>
                        {
                            Assert.Equal(3, fooList.Count);
                            //Query FooBar Custom Objects using Container name
                            var queryCommand = new QueryCommand<CustomObject>();
                            queryCommand.Where(customObject => customObject.Container == fooBarContainer);
                            var returnedSet = await client.ExecuteAsync(queryCommand);

                            Assert.NotNull(returnedSet);
                            Assert.Equal(5, returnedSet.Results.Count);
                            Assert.Equal(fooBarContainer, returnedSet.Results[0].Container);
                        });
                });
        }

        [Fact]
        public async Task QueryAllCustomObjects()
        {
            //Create container with 5 Custom FooBar Objects and container with 3 Foo Objects
            string fooBarContainer = $"FooBarContainer{TestingUtility.RandomInt()}";
            string fooContainer = $"FooContainer{TestingUtility.RandomInt()}";

            await WithListOfCustomObject<FooBar>(client,
                customObjectDraft => DefaultCustomObjectDraftWithContainerName(customObjectDraft, fooBarContainer),
                5, async fooBarList =>
                {
                    Assert.Equal(5, fooBarList.Count);
                    await WithListOfCustomObject<Foo>(client,
                        customObjectDraft => DefaultCustomObjectDraftWithContainerName(customObjectDraft, fooContainer),
                        3, async fooList =>
                        {
                            Assert.Equal(3, fooList.Count);
                            //Query All Custom Objects
                            var queryCommand = new QueryCommand<CustomObject>();
                            var returnedSet = await client.ExecuteAsync(queryCommand);

                            Assert.NotNull(returnedSet);
                            Assert.NotEmpty(returnedSet.Results);
                            var fooBarResults = returnedSet.Results
                                .Where(customObject => customObject.Container.Equals(fooBarContainer)).ToList();
                            var fooResults = returnedSet.Results
                                .Where(customObject => customObject.Container.Equals(fooContainer)).ToList();
                            Assert.Equal(5, fooBarResults.Count);
                            Assert.Equal(3, fooResults.Count);


                            var fooBarCustomObject = fooBarResults[0].GetValueAs<FooBar>();
                            var fooCustomObject = fooResults[0].GetValueAs<Foo>();

                            Assert.NotNull(fooBarCustomObject);
                            Assert.NotNull(fooCustomObject);
                        });
                });
        }

        [Fact]
        public async Task QueryDifferentTypesOfCustomObjectsInTheSameContainer()
        {
            string containerName = $"MixedContainer{TestingUtility.RandomInt()}";

            await WithListOfCustomObject<FooBar>(client,
                customObjectDraft => DefaultCustomObjectDraftWithContainerName(customObjectDraft, containerName),
                2, async fooBarList =>
                {
                    Assert.Equal(2, fooBarList.Count);
                    await WithListOfCustomObject<Foo>(client,
                        customObjectDraft =>
                            DefaultCustomObjectDraftWithContainerName(customObjectDraft, containerName),
                        3, async fooList =>
                        {
                            Assert.Equal(3, fooList.Count);
                            //Query All Custom Objects
                            var queryCommand = new QueryCommand<CustomObject>();
                            queryCommand.Where(customObject => customObject.Container == containerName);
                            var returnedSet = await client.ExecuteAsync(queryCommand);

                            Assert.NotNull(returnedSet);
                            Assert.Equal(5, returnedSet.Results.Count);
                        });
                });
        }
    }
}