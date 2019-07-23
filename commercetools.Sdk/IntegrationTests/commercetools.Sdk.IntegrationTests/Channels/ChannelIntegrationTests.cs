using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Channels.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Channels.ChannelsFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using ChangeKeyUpdateAction = commercetools.Sdk.Domain.Channels.UpdateActions.ChangeKeyUpdateAction;

namespace commercetools.Sdk.IntegrationTests.Channels
{
    [Collection("Integration Tests")]
    public class ChannelIntegrationTests
    {
        private readonly IClient client;

        public ChannelIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateChannel()
        {
            var key = $"CreateChannel-{TestingUtility.RandomString()}";
            await WithChannel(
                client, channelDraft => DefaultChannelDraftWithKey(channelDraft, key),
                channel => { Assert.Equal(key, channel.Key); });
        }

        [Fact]
        public async Task GetChannelById()
        {
            var key = $"GetChannelById-{TestingUtility.RandomString()}";
            await WithChannel(
                client, channelDraft => DefaultChannelDraftWithKey(channelDraft, key),
                async channel =>
                {
                    var retrievedChannel = await client
                        .ExecuteAsync(new GetByIdCommand<Channel>(channel.Id));
                    Assert.Equal(key, retrievedChannel.Key);
                });
        }

        [Fact]
        public async Task QueryChannels()
        {
            var key = $"QueryChannels-{TestingUtility.RandomString()}";
            await WithChannel(
                client, channelDraft => DefaultChannelDraftWithKey(channelDraft, key),
                async channel =>
                {
                    var queryCommand = new QueryCommand<Channel>();
                    queryCommand.Where(p => p.Key == channel.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteChannelById()
        {
            var key = $"DeleteChannelById-{TestingUtility.RandomString()}";
            await WithChannel(
                client, channelDraft => DefaultChannelDraftWithKey(channelDraft, key),
                async channel =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<Channel>(channel));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Channel>(channel))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateChannelChangeKey()
        {
            var newKey = $"UpdateChannelSetKey-{TestingUtility.RandomString()}";
            await WithUpdateableChannel(client, async channel =>
            {
                var updateActions = new List<UpdateAction<Channel>>();
                var action = new ChangeKeyUpdateAction
                {
                    Key = newKey
                };
                updateActions.Add(action);

                var updatedChannel = await client
                    .ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                Assert.Equal(newKey, updatedChannel.Key);
                return updatedChannel;
            });
        }

        [Fact]
        public async Task UpdateChannelChangeName()
        {
            await WithUpdateableChannel(client, async channel =>
            {
                var newName = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<Channel>>();
                var action = new ChangeNameUpdateAction {Name = newName};
                updateActions.Add(action);

                var updatedChannel = await client
                    .ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                Assert.Equal(newName["en"], updatedChannel.Name["en"]);
                return updatedChannel;
            });
        }

        [Fact]
        public async Task UpdateChannelChangeDescription()
        {
            await WithUpdateableChannel(client, async channel =>
            {
                var newDescription = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<Channel>>();
                var action = new ChangeDescriptionUpdateAction {Description = newDescription};
                updateActions.Add(action);

                var updatedChannel = await client
                    .ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                Assert.Equal(newDescription["en"], updatedChannel.Description["en"]);
                return updatedChannel;
            });
        }

        [Fact]
        public async Task UpdateChannelSetRoles()
        {
            await WithUpdateableChannel(client,
                channelDraft => DefaultChannelDraftWithRoles(channelDraft, new List<ChannelRole>
                {
                    ChannelRole.InventorySupply
                }),
                async channel =>
                {
                    Assert.Single(channel.Roles);
                    Assert.Equal(ChannelRole.InventorySupply, channel.Roles[0]);
                    var newRoles = new List<ChannelRole>
                    {
                        ChannelRole.OrderExport,
                        ChannelRole.ProductDistribution
                    };
                    var updateActions = new List<UpdateAction<Channel>>();
                    var action = new SetRolesUpdateAction
                    {
                        Roles = newRoles
                    };
                    updateActions.Add(action);

                    var updatedChannel = await client
                        .ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                    Assert.Equal(newRoles, updatedChannel.Roles);
                    return updatedChannel;
                });
        }

        [Fact]
        public async Task UpdateChannelAddRoles()
        {
            await WithUpdateableChannel(client,
                channelDraft => DefaultChannelDraftWithRoles(channelDraft, new List<ChannelRole>
                {
                    ChannelRole.InventorySupply
                }),
                async channel =>
                {
                    Assert.Single(channel.Roles);
                    Assert.Equal(ChannelRole.InventorySupply, channel.Roles[0]);

                    var addedRoles = new List<ChannelRole> { ChannelRole.OrderExport };
                    var updateActions = new List<UpdateAction<Channel>>();
                    var action = new AddRolesUpdateAction { Roles = addedRoles };
                    updateActions.Add(action);

                    var updatedChannel = await client
                        .ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                    Assert.Equal(2, updatedChannel.Roles.Count);
                    Assert.Contains(updatedChannel.Roles, role => role == addedRoles[0]);
                    return updatedChannel;
                });
        }

        [Fact]
        public async Task UpdateChannelRemoveRoles()
        {
            await WithUpdateableChannel(client,
                channelDraft => DefaultChannelDraftWithRoles(channelDraft, new List<ChannelRole>
                {
                    ChannelRole.InventorySupply,
                    ChannelRole.ProductDistribution
                }),
                async channel =>
                {
                    Assert.Equal(2, channel.Roles.Count);

                    var removedRoles = new List<ChannelRole> { ChannelRole.ProductDistribution };
                    var updateActions = new List<UpdateAction<Channel>>();
                    var action = new RemoveRolesUpdateAction { Roles = removedRoles };
                    updateActions.Add(action);

                    var updatedChannel = await client
                        .ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                    Assert.Single(updatedChannel.Roles);
                    Assert.Equal(ChannelRole.InventorySupply, updatedChannel.Roles[0]);
                    return updatedChannel;
                });
        }

        [Fact]
        public async Task UpdateChannelSetAddress()
        {
            await WithUpdateableChannel(client, async channel =>
            {
                var address = TestingUtility.GetRandomAddress();
                var updateActions = new List<UpdateAction<Channel>>();
                var action = new SetAddressUpdateAction
                {
                    Address = address
                };
                updateActions.Add(action);

                var updatedChannel = await client
                    .ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                Assert.Equal(address.Key, updatedChannel.Address.Key);
                Assert.Equal(address.State, updatedChannel.Address.State);
                Assert.Equal(address.Country, updatedChannel.Address.Country);
                return updatedChannel;
            });
        }

        [Fact]
        public async Task UpdateChannelSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableChannel(client,
                    async channel =>
                    {
                        var updateActions = new List<UpdateAction<Channel>>();
                        var setTypeAction = new SetCustomTypeUpdateAction
                        {
                            Type = new ResourceIdentifier<Type>
                            {
                                Key = type.Key
                            },
                            Fields = fields
                        };
                        updateActions.Add(setTypeAction);

                        var updatedChannel = await client.ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                        Assert.Equal(type.Id, updatedChannel.Custom.Type.Id);
                        return updatedChannel;
                    });
            });
        }

        [Fact]
        public async Task UpdateChannelSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableChannel(client,
                    channelDraft => DefaultChannelDraftWithCustomType(channelDraft, type, fields),
                    async channel =>
                    {
                        var updateActions = new List<UpdateAction<Channel>>();
                        var action = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };
                        updateActions.Add(action);

                        var updatedChannel = await client.ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                        Assert.Equal(newValue, updatedChannel.Custom.Fields["string-field"]);
                        return updatedChannel;
                    });
            });
        }

        [Fact]
        public async Task UpdateChannelSetGeoLocation()
        {
            await WithUpdateableChannel(client, async channel =>
            {
                var longitude = 13.37774;
                var latitude = 52.51627;
                var geoLocation = new GeoJsonGeometry(longitude, latitude);
                var updateActions = new List<UpdateAction<Channel>>();
                var action = new SetGeoLocationUpdateAction
                {
                    GeoLocation = geoLocation
                };
                updateActions.Add(action);

                var updatedChannel = await client
                    .ExecuteAsync(new UpdateByIdCommand<Channel>(channel, updateActions));

                Assert.Equal(longitude, updatedChannel.GeoLocation.Coordinates[0]);
                Assert.Equal(latitude, updatedChannel.GeoLocation.Coordinates[1]);
                return updatedChannel;
            });
        }

        #endregion
    }
}
