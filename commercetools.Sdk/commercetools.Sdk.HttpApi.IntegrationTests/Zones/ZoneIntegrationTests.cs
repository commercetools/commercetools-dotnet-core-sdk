using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Zones;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Zones
{
    [Collection("Integration Tests")]
    public class ZoneIntegrationTests : IClassFixture<ZonesFixture>
    {
        private readonly ZonesFixture zonesFixture;

        public ZoneIntegrationTests(ZonesFixture zonesFixture)
        {
            this.zonesFixture = zonesFixture;
        }

        [Fact]
        public void CreateZone()
        {
            IClient commerceToolsClient = this.zonesFixture.GetService<IClient>();
            ZoneDraft zoneDraft = this.zonesFixture.GetZoneDraft();
            Zone zone = commerceToolsClient
                .ExecuteAsync(new CreateCommand<Zone>(zoneDraft)).Result;
            this.zonesFixture.ZonesToDelete.Add(zone);
            Assert.Equal(zoneDraft.Key, zone.Key);
        }
    }
}
