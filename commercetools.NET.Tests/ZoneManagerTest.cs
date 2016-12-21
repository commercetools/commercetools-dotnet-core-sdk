using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Project;
using commercetools.Zones;

using NUnit.Framework;

using Newtonsoft.Json.Linq;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the ZoneManager class.
    /// </summary>
    [TestFixture]
    public class ZoneManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Zone> _testZones;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            _testZones = new List<Zone>();

            for (int i = 0; i < 5; i++)
            {
                ZoneDraft zoneDraft = Helper.GetTestZoneDraft(_project);
                Task<Response<Zone>> zoneTask = _client.Zones().CreateZoneAsync(zoneDraft);
                zoneTask.Wait();
                Assert.IsTrue(zoneTask.Result.Success);

                Zone zone = zoneTask.Result.Result;
                Assert.NotNull(zone.Id);

                _testZones.Add(zone);
            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (Zone zone in _testZones)
            {
                Task task = _client.Zones().DeleteZoneAsync(zone);
                task.Wait();
            }
        }

        /// <summary>
        /// Tests the ZoneManager.GetZoneByIdAsync method.
        /// </summary>
        /// <see cref="ZoneManager.GetZoneByIdAsync"/>
        [Test]
        public async Task ShouldGetZoneByIdAsync()
        {
            Response<Zone> response = await _client.Zones().GetZoneByIdAsync(_testZones[0].Id);
            Assert.IsTrue(response.Success);

            Zone zone = response.Result;
            Assert.NotNull(zone);
            Assert.AreEqual(zone.Id, _testZones[0].Id);
        }

        /// <summary>
        /// Tests the ZoneManager.QueryZonesAsync method.
        /// </summary>
        /// <see cref="ZoneManager.QueryZonesAsync"/>
        [Test]
        public async Task ShouldQueryZonesAsync()
        {
            Response<ZoneQueryResult> response = await _client.Zones().QueryZonesAsync();
            Assert.IsTrue(response.Success);

            ZoneQueryResult zoneQueryResult = response.Result;
            Assert.NotNull(zoneQueryResult.Results);
            Assert.GreaterOrEqual(zoneQueryResult.Results.Count, 1);

            int limit = 2;
            response = await _client.Zones().QueryZonesAsync(limit: limit);
            Assert.IsTrue(response.Success);

            zoneQueryResult = response.Result;
            Assert.NotNull(zoneQueryResult.Results);
            Assert.LessOrEqual(zoneQueryResult.Results.Count, limit);
        }

        /// <summary>
        /// Tests the ZoneManager.CreateZoneAsync and ZoneManager.DeleteZoneAsync methods.
        /// </summary>
        /// <see cref="ZoneManager.CreateZoneAsync"/>
        /// <seealso cref="ZoneManager.DeleteZoneAsync(commercetools.Zones.Zone)"/>
        [Test]
        public async Task ShouldCreateAndDeleteZoneAsync()
        {
            ZoneDraft zoneDraft = Helper.GetTestZoneDraft(_project);
            Response<Zone> response = await _client.Zones().CreateZoneAsync(zoneDraft);
            Assert.IsTrue(response.Success);

            Zone zone = response.Result;
            Assert.NotNull(zone.Id);

            string deletedZoneId = zone.Id;

            Response<JObject> deleteResponse = await _client.Zones().DeleteZoneAsync(zone);
            Assert.IsTrue(deleteResponse.Success);

            response = await _client.Zones().GetZoneByIdAsync(deletedZoneId);
            Assert.IsFalse(response.Success);
        }

        /// <summary>
        /// Tests the ZoneManager.UpdateZoneAsync method.
        /// </summary>
        /// <see cref="ZoneManager.UpdateZoneAsync(commercetools.Zones.Zone, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdateZoneAsync()
        {
            string newName = string.Concat("Test Zone ", Helper.GetRandomString(10));
            string newDescription = string.Concat("Test Description ", Helper.GetRandomString(10));

            GenericAction changeNameAction = new GenericAction("changeName");
            changeNameAction.SetProperty("name", newName);

            GenericAction setDescriptionAction = new GenericAction("setDescription");
            changeNameAction.SetProperty("description", newDescription);

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(changeNameAction);
            actions.Add(setDescriptionAction);

            Response<Zone> response = await _client.Zones().UpdateZoneAsync(_testZones[1], actions);
            Assert.IsTrue(response.Success);

            _testZones[1] = response.Result;
            Assert.NotNull(_testZones[1].Id);

            foreach (string language in _project.Languages)
            {
                Assert.AreEqual(_testZones[1].Name, newName);
                Assert.AreEqual(_testZones[1].Description, newDescription);
            }
        }
    }
}
