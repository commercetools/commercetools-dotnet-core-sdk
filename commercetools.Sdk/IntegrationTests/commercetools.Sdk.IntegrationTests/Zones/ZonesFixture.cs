using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Zones;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.Zones
{
    public static class ZonesFixture
    {
        #region DraftBuilds
        public static ZoneDraft DefaultZoneDraft(ZoneDraft zoneDraft)
        {
            var random = TestingUtility.RandomInt();
            zoneDraft.Key = $"key_{random}";
            zoneDraft.Name = $"Name_{random}";
            return zoneDraft;
        }
        public static ZoneDraft DefaultZoneDraftWithKey(ZoneDraft draft, string key)
        {
            var zoneDraft = DefaultZoneDraft(draft);
            zoneDraft.Key = key;
            return zoneDraft;
        }
        public static ZoneDraft DefaultZoneWithEmptyLocations(ZoneDraft draft)
        {
            var zoneDraft = DefaultZoneDraft(draft);
            zoneDraft.Locations = new List<Location>();
            return zoneDraft;
        }
        public static ZoneDraft DefaultZoneWithOneLocation(ZoneDraft draft, Location location)
        {
            var zoneDraft = DefaultZoneDraft(draft);
            zoneDraft.Locations = new List<Location> { location };
            return zoneDraft;
        }

        #endregion


        #region WithZone

        public static async Task WithZone( IClient client, Action<Zone> func)
        {
            await With(client, new ZoneDraft(), DefaultZoneDraft, func);
        }
        public static async Task WithZone( IClient client, Func<ZoneDraft, ZoneDraft> draftAction, Action<Zone> func)
        {
            await With(client, new ZoneDraft(), draftAction, func);
        }

        public static async Task WithZone( IClient client, Func<Zone, Task> func)
        {
            await WithAsync(client, new ZoneDraft(), DefaultZoneDraft, func);
        }
        public static async Task WithZone( IClient client, Func<ZoneDraft, ZoneDraft> draftAction, Func<Zone, Task> func)
        {
            await WithAsync(client, new ZoneDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableZone

        public static async Task WithUpdateableZone(IClient client, Func<Zone, Zone> func)
        {
            await WithUpdateable(client, new ZoneDraft(), DefaultZoneDraft, func);
        }

        public static async Task WithUpdateableZone(IClient client, Func<ZoneDraft, ZoneDraft> draftAction, Func<Zone, Zone> func)
        {
            await WithUpdateable(client, new ZoneDraft(), draftAction, func);
        }

        public static async Task WithUpdateableZone(IClient client, Func<Zone, Task<Zone>> func)
        {
            await WithUpdateableAsync(client, new ZoneDraft(), DefaultZoneDraft, func);
        }
        public static async Task WithUpdateableZone(IClient client, Func<ZoneDraft, ZoneDraft> draftAction, Func<Zone, Task<Zone>> func)
        {
            await WithUpdateableAsync(client, new ZoneDraft(), draftAction, func);
        }

        #endregion
    }
}
