using System;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomObject;
using commercetools.Sdk.Domain.Zones;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using commercetools.Sdk.HttpApi.Extensions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.CustomObjects
{
    [Collection("Integration Tests")]
    public class CustomObjectsIntegrationTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly CustomObjectsFixture customObjectsFixture;

        public CustomObjectsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.customObjectsFixture = new CustomObjectsFixture(serviceProviderFixture);
        }

        [Fact]
        public void CreateCustomObjectAsFooBar()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();

            FooBar fooBar = new FooBar();
            string key = TestingUtility.RandomString(10);

            CustomObjectDraft<FooBar> customObjectDraft =
                this.customObjectsFixture.GetFooBarCustomObjectDraft(TestingUtility.DefaultContainerName, key,
                    fooBar);
            var customObject = commerceToolsClient
                .ExecuteAsync(new CustomObjectUpsertCommand<FooBar>(customObjectDraft)).Result;
            this.customObjectsFixture.CustomObjectsToDelete.Add(customObject);
            Assert.Equal(customObjectDraft.Key, customObject.Key);
            Assert.Equal(fooBar.Bar, customObject.Value.Bar);
        }

        [Fact]
        public void CreateCustomObjectAsString()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();
            var customObjectDraft =
                this.customObjectsFixture.GetCustomObjectDraft(TestingUtility.DefaultContainerName, TestingUtility.RandomString(10),
                    "test");
            var customObject = commerceToolsClient
                .ExecuteAsync(new CustomObjectUpsertCommand<string>(customObjectDraft)).Result;
            this.customObjectsFixture.CustomObjectsToDelete.Add(customObject);
            Assert.Equal(customObjectDraft.Key, customObject.Key);
            Assert.Equal(customObjectDraft.Value, customObject.Value);
        }

        [Fact]
        public void GetCustomObjectById()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();
            var customObject = this.customObjectsFixture.CreateFooBarCustomObject();
            var retrievedCustomObject = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<CustomObject>(new Guid(customObject.Id))).Result;
            this.customObjectsFixture.CustomObjectsToDelete.Add(customObject);
            Assert.Equal(customObject.Key, retrievedCustomObject.Key);
            var fooBarCustomObject = retrievedCustomObject.GetValueAs<FooBar>();

            Assert.NotNull(fooBarCustomObject);
            Assert.Equal(customObject.Value.Bar, fooBarCustomObject.Bar);
        }

        [Fact]
        public void GetCustomObjectByContainerAndKey()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();
            var customObject = this.customObjectsFixture.CreateFooBarCustomObject();
            var retrievedCustomObject = commerceToolsClient
                .ExecuteAsync(new GetByContainerAndKeyCommand<CustomObject>(customObject.Container, customObject.Key))
                .Result;
            this.customObjectsFixture.CustomObjectsToDelete.Add(customObject);
            Assert.Equal(customObject.Container, retrievedCustomObject.Container);
            Assert.Equal(customObject.Key, retrievedCustomObject.Key);

            var fooBarCustomObject = retrievedCustomObject.GetValueAs<FooBar>();

            Assert.NotNull(fooBarCustomObject);
            Assert.Equal(customObject.Value.Bar, fooBarCustomObject.Bar);
        }

        [Fact]
        public void UpdateCustomObject()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();

            FooBar fooBar = new FooBar();
            string key = TestingUtility.RandomString(10);

            CustomObjectDraft<FooBar> customObjectDraft =
                this.customObjectsFixture.GetFooBarCustomObjectDraft(TestingUtility.DefaultContainerName, key, fooBar);

            //Create FooBar Custom Object
            var customObject = commerceToolsClient
                .ExecuteAsync(new CustomObjectUpsertCommand<FooBar>(customObjectDraft)).Result;

            Assert.Equal(customObjectDraft.Key, customObject.Key);
            Assert.Equal(customObjectDraft.Value.Bar, customObject.Value.Bar);

            //Then Update Custom Object Value
            customObjectDraft.Value.Bar = TestingUtility.RandomString(10);

            var updatedCustomObject = commerceToolsClient
                .ExecuteAsync(new CustomObjectUpsertCommand<FooBar>(customObjectDraft)).Result;

            this.customObjectsFixture.CustomObjectsToDelete.Add(updatedCustomObject);

            Assert.Equal(customObjectDraft.Key, updatedCustomObject.Key);
            Assert.Equal(customObjectDraft.Value.Bar, updatedCustomObject.Value.Bar);
        }

        [Fact]
        public async void DeleteCustomObjectById()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();
            var customObject = this.customObjectsFixture.CreateFooBarCustomObject();

            CustomObject deletedCustomObject = await commerceToolsClient
                .ExecuteAsync(
                    new DeleteByIdCommand<CustomObject>(new Guid(customObject.Id), customObject.Version));

            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<CustomObject>(new Guid(deletedCustomObject.Id))));

            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public async void DeleteCustomObjectByContainerAndKey()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();
            var customObject = this.customObjectsFixture.CreateFooBarCustomObject();

            CustomObject deletedCustomObject = await commerceToolsClient
                .ExecuteAsync(
                    new DeleteByContainerAndKeyCommand<CustomObject>(customObject.Container, customObject.Key));

            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<CustomObject>(new Guid(deletedCustomObject.Id))));

            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void QueryCustomObjectsByContainer()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();

            //Create 5 FooBar CustomObjects in container "FooBarContainer"
            string fooBarContainerName = $"FooBarContainer{TestingUtility.RandomInt()}";
            var customObjectsInFooBarContainer = customObjectsFixture.CreateMultipleFooBarCustomObjects(fooBarContainerName, 5);

            //Create 3 FooBar customObjects in other Container Name
            string otherContainerName = $"OtherContainer{TestingUtility.RandomInt()}";
            var customObjectsInOtherContainer = customObjectsFixture.CreateMultipleFooBarCustomObjects(otherContainerName, 3);

            Assert.Equal(5, customObjectsInFooBarContainer.Count);
            Assert.Equal(3, customObjectsInOtherContainer.Count);

            //Query FooBar Custom Objects using Container name
            var queryCommand = new QueryCommand<CustomObject>();
            queryCommand.Where(customObject => customObject.Container == fooBarContainerName);
            var returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;

            this.customObjectsFixture.CustomObjectsToDelete.AddRange(customObjectsInFooBarContainer);
            this.customObjectsFixture.CustomObjectsToDelete.AddRange(customObjectsInOtherContainer);

            Assert.NotNull(returnedSet);
            Assert.Equal(5, returnedSet.Results.Count);
            Assert.Equal(fooBarContainerName, returnedSet.Results[0].Container);
        }

        [Fact]
        public void QueryAllCustomObjects()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();

            //Create 5 FooBar CustomObjects in container "FooBarContainer"
            string fooBarContainerName = $"FooBarContainer{TestingUtility.RandomInt()}";
            var customObjectsInFooBarContainer = customObjectsFixture.CreateMultipleFooBarCustomObjects(fooBarContainerName, 5);

            //Create 3 FooBar customObjects in other Container Name
            string otherContainerName = $"OtherContainer{TestingUtility.RandomInt()}";
            var customObjectsInOtherContainer = customObjectsFixture.CreateMultipleFooBarCustomObjects(otherContainerName, 3);

            Assert.Equal(5, customObjectsInFooBarContainer.Count);
            Assert.Equal(3, customObjectsInOtherContainer.Count);

            //Query FooBar Custom Objects using Container name
            var queryCommand = new QueryCommand<CustomObject>();
            var returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;

            this.customObjectsFixture.CustomObjectsToDelete.AddRange(customObjectsInFooBarContainer);
            this.customObjectsFixture.CustomObjectsToDelete.AddRange(customObjectsInOtherContainer);

            Assert.NotNull(returnedSet);
            Assert.NotEmpty(returnedSet.Results);
            Assert.True(returnedSet.Results.Count > customObjectsInFooBarContainer.Count);
        }

        [Fact]
        public void QueryDifferentTypesOfCustomObjectsByContainer()
        {
            IClient commerceToolsClient = this.customObjectsFixture.GetService<IClient>();

            //Create 2 CustomObjects with different types in container "MixedContainer"
            string containerName = $"MixedContainer{TestingUtility.RandomInt()}";
            var customObjectsInMixedContainer = customObjectsFixture.CreateCustomObjectsWithDifferentTypes(containerName);

            Assert.Equal(2, customObjectsInMixedContainer.Count);
            Assert.IsType<CustomObject<Foo>>(customObjectsInMixedContainer[0]);
            Assert.IsType<CustomObject<FooBar>>(customObjectsInMixedContainer[1]);

            //Query FooBar Custom Objects using Container name
            var queryCommand = new QueryCommand<CustomObject>();
            queryCommand.Where(customObject => customObject.Container == containerName);
            var returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;

            this.customObjectsFixture.CustomObjectsToDelete.AddRange(customObjectsInMixedContainer);

            Assert.NotNull(returnedSet);
            Assert.Equal(2, returnedSet.Results.Count);

            var fooCustomObject = returnedSet.Results[0].GetValueAs<Foo>();
            var fooBarCustomObject = returnedSet.Results[1].GetValueAs<FooBar>();

            Assert.NotNull(fooCustomObject);
            Assert.NotNull(fooBarCustomObject);
        }
    }
}
