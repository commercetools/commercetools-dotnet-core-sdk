using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.APIExtensions;
using commercetools.Sdk.Domain.APIExtensions.UpdateActions;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.APIExtensions.ApiExtensionsFixture;

namespace commercetools.Sdk.IntegrationTests.APIExtensions
{
    [Collection("Integration Tests")]
    public class ApiExtensionsIntegrationTests
    {
        private readonly IClient client;

        public ApiExtensionsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateExtension()
        {
            await WithExtension(
                client,
                extension =>
                {
                    Assert.NotNull(extension.Destination);
                    Assert.Single(extension.Triggers);
                    var trigger = extension.Triggers[0];
                    Assert.Equal(ExtensionResourceType.Customer, trigger.ResourceTypeId);
                });
        }

        [Fact]
        public async Task CreateExtensionForCart()
        {
            await WithExtension(
                client,
                extensionDraft =>
                    DefaultExtensionDraftForSpecificResourceType(extensionDraft, ExtensionResourceType.Cart),
                extension =>
                {
                    Assert.NotNull(extension.Destination);
                    Assert.Single(extension.Triggers);
                    var trigger = extension.Triggers[0];
                    Assert.Equal(ExtensionResourceType.Cart, trigger.ResourceTypeId);
                });
        }

        [Fact]
        public async Task GetExtensionById()
        {
            await WithExtension(
                client,
                async extension =>
                {
                    var retrievedExtension = await client
                        .ExecuteAsync(new GetByIdCommand<Extension>(extension.Id));
                    Assert.NotNull(retrievedExtension);
                    Assert.Equal(extension.Id, retrievedExtension.Id);
                });
        }

        [Fact]
        public async Task GetExtensionByKey()
        {
            await WithExtension(
                client,
                async extension =>
                {
                    var retrievedExtension = await client
                        .ExecuteAsync(new GetByKeyCommand<Extension>(extension.Key));
                    Assert.NotNull(retrievedExtension);
                    Assert.Equal(extension.Key, retrievedExtension.Key);
                });
        }

        [Fact]
        public async Task QueryExtensions()
        {
            await WithExtension(
                client,
                async extension =>
                {
                    var queryCommand = new QueryCommand<Extension>();
                    queryCommand.Where(p => p.Key == extension.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(extension.Key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteExtensionById()
        {
            await WithExtension(
                client,
                async extension =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<Extension>(extension));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Extension>(extension))
                    );
                });
        }

        [Fact]
        public async Task DeleteExtensionByKey()
        {
            await WithExtension(
                client,
                async extension =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<Extension>(extension));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Extension>(extension))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateExtensionSetKey()
        {
            var newKey = $"UpdateExtensionSetKey-{TestingUtility.RandomString()}";
            await WithUpdateableExtension(client, async extension =>
            {
                var updateActions = new List<UpdateAction<Extension>>();
                var setKeyAction = new SetKeyUpdateAction { Key = newKey };
                updateActions.Add(setKeyAction);

                var updatedExtension = await client
                    .ExecuteAsync(new UpdateByIdCommand<Extension>(extension, updateActions));

                Assert.Equal(newKey, updatedExtension.Key);
                return updatedExtension;
            });
        }

        [Fact]
        public async Task UpdateExtensionByKeySetTimeoutInMs()
        {
            await WithUpdateableExtension(client, async extension =>
            {
                var timeOutInMs = 1000;
                Assert.Null(extension.TimeoutInMs);
                var updateActions = new List<UpdateAction<Extension>>();
                var setTimeoutAction = new SetTimeoutInMsUpdateAction { TimeoutInMs = timeOutInMs };
                updateActions.Add(setTimeoutAction);

                var updatedExtension = await client
                    .ExecuteAsync(new UpdateByKeyCommand<Extension>(extension.Key, extension.Version, updateActions));

                Assert.Equal(timeOutInMs, updatedExtension.TimeoutInMs);
                return updatedExtension;
            });
        }

        [Fact]
        public async Task UpdateExtensionChangeTriggers()
        {
            await WithUpdateableExtension(client,
                extensionDraft =>
                    DefaultExtensionDraftForSpecificResourceType(extensionDraft, ExtensionResourceType.Cart),
                async extension =>
                {
                    Assert.Single(extension.Triggers);
                    var trigger = extension.Triggers[0];
                    Assert.Equal(ExtensionResourceType.Cart, trigger.ResourceTypeId);
                    var updateActions = new List<UpdateAction<Extension>>();
                    var changeTriggersAction = new ChangeTriggersUpdateAction
                    {
                        Triggers = new List<Trigger>
                        {
                            new Trigger
                            {
                                Actions = new List<TriggerType> {TriggerType.Update},
                                ResourceTypeId = ExtensionResourceType.Customer
                            }
                        }
                    };
                    updateActions.Add(changeTriggersAction);

                    var updatedExtension = await client
                        .ExecuteAsync(new UpdateByIdCommand<Extension>(extension, updateActions));

                    Assert.Single(updatedExtension.Triggers);
                    var updatedTrigger = updatedExtension.Triggers[0];

                    Assert.NotEqual(trigger.ResourceTypeId, updatedTrigger.ResourceTypeId);
                    Assert.False(trigger.Actions.SequenceEqual(updatedTrigger.Actions));
                    Assert.Equal(ExtensionResourceType.Customer, updatedTrigger.ResourceTypeId);
                    return updatedExtension;
                });
        }

        [Fact]
        public async Task UpdateExtensionChangeDestination()
        {
            await WithUpdateableExtension(client, async extension =>
            {
                var headerValue = "newhZGRpbjpvcGVuIHNlc2FtZQ==";
                var newDestination = new HttpDestination
                {
                    Url = "http://www.new-destination.com/",
                    Authentication = new AuthorizationHeader
                    {
                        HeaderValue = headerValue
                    }
                };
                var updateActions = new List<UpdateAction<Extension>>();
                var changeDestinationAction = new ChangeDestinationUpdateAction
                {
                    Destination = newDestination
                };
                updateActions.Add(changeDestinationAction);

                var updatedExtension = await client
                    .ExecuteAsync(new UpdateByIdCommand<Extension>(extension, updateActions));

                var updatedDestination = updatedExtension.Destination as HttpDestination;
                Assert.NotNull(updatedDestination);
                Assert.Equal(newDestination.Url, updatedDestination.Url);
                var updatedAuthentication = updatedDestination.Authentication as AuthorizationHeader;
                Assert.NotNull(updatedAuthentication);
                return updatedExtension;
            });
        }

        [Fact]
        public async Task UpdateExtensionChangeDestinationWithAzureFunction()
        {
            await WithUpdateableExtension(client, async extension =>
            {
                var newDestination = new HttpDestination
                {
                    Url = "http://www.new-destination.com/",
                    Authentication = new AzureFunctionsAuthentication { Key = "default-Key" }
                };
                var updateActions = new List<UpdateAction<Extension>>();
                var changeDestinationAction = new ChangeDestinationUpdateAction
                {
                    Destination = newDestination
                };
                updateActions.Add(changeDestinationAction);

                var updatedExtension = await client
                    .ExecuteAsync(new UpdateByIdCommand<Extension>(extension, updateActions));

                var updatedDestination = updatedExtension.Destination as HttpDestination;
                Assert.NotNull(updatedDestination);
                Assert.Equal(newDestination.Url, updatedDestination.Url);
                var updatedAuthentication = updatedDestination.Authentication as AzureFunctionsAuthentication;
                Assert.NotNull(updatedAuthentication);
                return updatedExtension;
            });
        }


        #endregion
    }
}
