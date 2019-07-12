using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Zones;
using commercetools.Sdk.Domain.Zones.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Zones.ZonesFixture;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.Zones.UpdateActions.SetDescriptionUpdateAction;

namespace commercetools.Sdk.IntegrationTests.Zones
{
    [Collection("Integration Tests")]
    public class ZonesIntegrationTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly IClient client;

        public ZonesIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateZone()
        {
            var key = $"CreateZone-{TestingUtility.RandomString()}";
            await WithZone(
                client, zoneDraft => DefaultZoneDraftWithKey(zoneDraft, key),
                zone => { Assert.Equal(key, zone.Key); });
        }

        [Fact]
        public async Task GetZoneById()
        {
            var key = $"GetZoneById-{TestingUtility.RandomString()}";
            await WithZone(
                client, zoneDraft => DefaultZoneDraftWithKey(zoneDraft, key),
                async zone =>
                {
                    var retrievedZone = await client
                        .ExecuteAsync(new GetByIdCommand<Zone>(zone.Id));
                    Assert.Equal(key, retrievedZone.Key);
                });
        }

        [Fact]
        public async Task GetZoneByKey()
        {
            var key = $"GetZoneByKey-{TestingUtility.RandomString()}";
            await WithZone(
                client, zoneDraft => DefaultZoneDraftWithKey(zoneDraft, key),
                async zone =>
                {
                    var retrievedZone = await client
                        .ExecuteAsync(new GetByKeyCommand<Zone>(zone.Key));
                    Assert.Equal(key, retrievedZone.Key);
                });
        }

        [Fact]
        public async Task QueryZones()
        {
            var key = $"QueryZones-{TestingUtility.RandomString()}";
            await WithZone(
                client, zoneDraft => DefaultZoneDraftWithKey(zoneDraft, key),
                async zone =>
                {
                    var queryCommand = new QueryCommand<Zone>();
                    queryCommand.Where(p => p.Key == zone.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteZoneById()
        {
            var key = $"DeleteZoneById-{TestingUtility.RandomString()}";
            await WithZone(
                client, zoneDraft => DefaultZoneDraftWithKey(zoneDraft, key),
                async zone =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<Zone>(zone));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Zone>(zone))
                    );
                });
        }

        [Fact]
        public async Task DeleteZoneByKey()
        {
            var key = $"DeleteZoneByKey-{TestingUtility.RandomString()}";
            await WithZone(
                client, zoneDraft => DefaultZoneDraftWithKey(zoneDraft, key),
                async zone =>
                {
                    await client.ExecuteAsync(new DeleteByKeyCommand<Zone>(zone.Key, zone.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Zone>(zone))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateZoneSetKey()
        {
            var newKey = $"UpdateZoneSetKey-{TestingUtility.RandomString()}";
            await WithUpdateableZone(client, async zone =>
            {
                var updateActions = new List<UpdateAction<Zone>>();
                var setKeyAction = new SetKeyUpdateAction() {Key = newKey};
                updateActions.Add(setKeyAction);

                var updatedZone = await client
                    .ExecuteAsync(new UpdateByIdCommand<Zone>(zone, updateActions));

                Assert.Equal(newKey, updatedZone.Key);
                return updatedZone;
            });
        }

        [Fact]
        public async Task UpdateZoneChangeName()
        {
            var newName = $"UpdateZoneChangeName-{TestingUtility.RandomString()}";
            await WithUpdateableZone(client, async zone =>
            {
                var updateActions = new List<UpdateAction<Zone>>();
                var changeNameAction = new ChangeNameUpdateAction
                {
                    Name = newName
                };
                updateActions.Add(changeNameAction);

                var updatedZone = await client
                    .ExecuteAsync(new UpdateByIdCommand<Zone>(zone, updateActions));

                Assert.Equal(newName, updatedZone.Name);
                return updatedZone;
            });
        }

        [Fact]
        public async Task UpdateZoneSetDescription()
        {
            var newDescription = $"UpdateZoneSetDescription-{TestingUtility.RandomString()}";
            await WithUpdateableZone(client, async zone =>
            {
                var updateActions = new List<UpdateAction<Zone>>();
                var setDescriptionAction = new SetDescriptionUpdateAction
                {
                    Description = newDescription
                };
                updateActions.Add(setDescriptionAction);

                var updatedZone = await client
                    .ExecuteAsync(new UpdateByIdCommand<Zone>(zone, updateActions));

                Assert.Equal(newDescription, updatedZone.Description);
                return updatedZone;
            });
        }

        [Fact]
        public async Task UpdateZoneAddLocation()
        {
            var country = "DE";
            await WithUpdateableZone(client, DefaultZoneWithEmptyLocations ,async zone =>
            {
                Assert.Empty(zone.Locations);

                var updateActions = new List<UpdateAction<Zone>>();
                var addLocationAction = new AddLocationUpdateAction
                {
                    Location = new Location
                    {
                        Country = country
                    }
                };
                updateActions.Add(addLocationAction);

                var updatedZone = await client
                    .ExecuteAsync(new UpdateByIdCommand<Zone>(zone, updateActions));

                Assert.Single(updatedZone.Locations);
                Assert.Equal(country, updatedZone.Locations[0].Country);
                return updatedZone;
            });
        }

        [Fact]
        public async Task UpdateZoneRemoveLocation()
        {
            var location = new Location {Country = "DE"};
            await WithUpdateableZone(client, zoneDraft => DefaultZoneWithOneLocation(zoneDraft, location) ,
                async zone =>
            {
                Assert.Single(zone.Locations);

                var updateActions = new List<UpdateAction<Zone>>();
                var removeLocationAction = new RemoveLocationUpdateAction
                {
                    Location = location
                };
                updateActions.Add(removeLocationAction);

                var updatedZone = await client
                    .ExecuteAsync(new UpdateByIdCommand<Zone>(zone, updateActions));

                Assert.Empty(updatedZone.Locations);
                return updatedZone;
            });
        }

        #endregion
    }
}
