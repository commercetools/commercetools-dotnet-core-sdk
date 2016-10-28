using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Project;
using commercetools.ShippingMethods;
using commercetools.TaxCategories;
using commercetools.Zones;

using NUnit.Framework;

using Newtonsoft.Json.Linq;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the ShippingMethodManager class.
    /// </summary>
    [TestFixture]
    public class ShippingMethodManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<ShippingMethod> _testShippingMethods;
        private List<Zone> _testZones;
        private TaxCategory _testTaxCategory;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Project.Project> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            _project = projectTask.Result;

            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<TaxCategory> taxCategory = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            taxCategory.Wait();
            _testTaxCategory = taxCategory.Result;

            Assert.NotNull(_testTaxCategory.Id);

            _testZones = new List<Zone>();
            _testShippingMethods = new List<ShippingMethod>();

            for (int i = 0; i < 5; i++)
            {
                ZoneDraft zoneDraft = Helper.GetTestZoneDraft(_project);
                Task<Zone> zoneTask = _client.Zones().CreateZoneAsync(zoneDraft);
                zoneTask.Wait();
                Zone zone = zoneTask.Result;

                Assert.NotNull(zone.Id);

                _testZones.Add(zone);

                ShippingMethodDraft shippingMethodDraft = Helper.GetTestShippingMethodDraft(_project, _testTaxCategory, zone);
                Task<ShippingMethod> shippingMethodTask = _client.ShippingMethods().CreateShippingMethodAsync(shippingMethodDraft);
                shippingMethodTask.Wait();
                ShippingMethod shippingMethod = shippingMethodTask.Result;

                Assert.NotNull(shippingMethod.Id);

                _testShippingMethods.Add(shippingMethod);
            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            Task task;

            foreach (ShippingMethod shippingMethod in _testShippingMethods)
            {
                task = _client.ShippingMethods().DeleteShippingMethodAsync(shippingMethod);
                task.Wait();
            }

            task = _client.TaxCategories().DeleteTaxCategoryAsync(_testTaxCategory);
            task.Wait();

            foreach (Zone zone in _testZones)
            {
                task = _client.Zones().DeleteZoneAsync(zone);
                task.Wait();
            }
        }

        /// <summary>
        /// Tests the ShippingMethodManager.GetShippingMethodByIdAsync method.
        /// </summary>
        /// <see cref="ShippingMethodManager.GetShippingMethodByIdAsync"/>
        [Test]
        public async Task ShouldGetShippingMethodByIdAsync()
        {
            ShippingMethod shippingMethod = await _client.ShippingMethods().GetShippingMethodByIdAsync(_testShippingMethods[0].Id);

            Assert.NotNull(shippingMethod.Id);
            Assert.AreEqual(shippingMethod.Id, _testShippingMethods[0].Id);
        }

        /// <summary>
        /// Tests the ShippingMethodManager.QueryShippingMethodsAsync method.
        /// </summary>
        /// <see cref="ShippingMethodManager.QueryShippingMethodsAsync"/>
        [Test]
        public async Task ShouldQueryShippingMethodsAsync()
        {
            ShippingMethodQueryResult result = await _client.ShippingMethods().QueryShippingMethodsAsync();

            Assert.NotNull(result.Results);
            Assert.GreaterOrEqual(result.Results.Count, 1);

            int limit = 2;
            result = await _client.ShippingMethods().QueryShippingMethodsAsync(limit: limit);

            Assert.NotNull(result.Results);
            Assert.LessOrEqual(result.Results.Count, limit);
        }

        /// <summary>
        /// Tests the ShippingMethodManager.CreateShippingMethodAsync and ShippingMethodManager.DeleteShippingMethodAsync methods.
        /// </summary>
        /// <see cref="ShippingMethodManager.CreateShippingMethodAsync"/>
        /// <seealso cref="ShippingMethodManager.DeleteShippingMethodAsync(commercetools.ShippingMethods.ShippingMethod)"/>
        [Test]
        public async Task ShouldCreateAndDeleteShippingMethodAsync()
        {
            ShippingMethodDraft shippingMethodDraft = Helper.GetTestShippingMethodDraft(_project, _testTaxCategory, _testZones[0]);
            ShippingMethod shippingMethod = await _client.ShippingMethods().CreateShippingMethodAsync(shippingMethodDraft);

            Assert.NotNull(shippingMethod.Id);

            string deletedShippingMethodId = shippingMethod.Id;

            await _client.ShippingMethods().DeleteShippingMethodAsync(shippingMethod);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate
                {
                    Task task = _client.ShippingMethods().GetShippingMethodByIdAsync(deletedShippingMethodId);
                    task.Wait();
                });
        }

        /// <summary>
        /// Tests the ShippingMethodManager.UpdateShippingMethodAsync method.
        /// </summary>
        /// <see cref="ShippingMethodManager.UpdateShippingMethodAsync(commercetools.ShippingMethods.ShippingMethod, System.Collections.Generic.List{Newtonsoft.Json.Linq.JObject})"/>
        [Test]
        public async Task ShouldUpdateShippingMethodAsync()
        {
            List<JObject> actions = new List<JObject>();

            string newName = string.Concat("Test Shipping Method ", Helper.GetRandomString(10));
            string newDescription = string.Concat("Test Description ", Helper.GetRandomString(10));

            actions.Add(
                JObject.FromObject(new
                {
                    action = "changeName",
                    name = newName
                })
            );

            actions.Add(
                JObject.FromObject(new
                {
                    action = "setDescription",
                    description = newDescription
                })
            );

            _testShippingMethods[0] = await _client.ShippingMethods().UpdateShippingMethodAsync(_testShippingMethods[0], actions);

            Assert.NotNull(_testShippingMethods[0].Id);

            foreach (string language in _project.Languages)
            {
                Assert.AreEqual(_testShippingMethods[0].Name, newName);
                Assert.AreEqual(_testShippingMethods[0].Description, newDescription);
            }
        }
    }
}