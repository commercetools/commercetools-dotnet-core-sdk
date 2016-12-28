using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Project;
using commercetools.ShippingMethods;
using commercetools.TaxCategories;
using commercetools.Zones;

using NUnit.Framework;

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

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<Response<TaxCategory>> taxCategoryTask = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            taxCategoryTask.Wait();
            Assert.IsTrue(taxCategoryTask.Result.Success);

            _testTaxCategory = taxCategoryTask.Result.Result;
            Assert.NotNull(_testTaxCategory.Id);

            _testZones = new List<Zone>();
            _testShippingMethods = new List<ShippingMethod>();

            for (int i = 0; i < 5; i++)
            {
                ZoneDraft zoneDraft = Helper.GetTestZoneDraft();
                Task<Response<Zone>> zoneTask = _client.Zones().CreateZoneAsync(zoneDraft);
                zoneTask.Wait();
                Assert.IsTrue(zoneTask.Result.Success);

                Zone zone = zoneTask.Result.Result;
                Assert.NotNull(zone.Id);

                _testZones.Add(zone);

                ShippingMethodDraft shippingMethodDraft = Helper.GetTestShippingMethodDraft(_project, _testTaxCategory, zone);
                Task<Response<ShippingMethod>> shippingMethodTask = _client.ShippingMethods().CreateShippingMethodAsync(shippingMethodDraft);
                shippingMethodTask.Wait();
                Assert.IsTrue(shippingMethodTask.Result.Success);

                ShippingMethod shippingMethod = shippingMethodTask.Result.Result;
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
            Response<ShippingMethod> response = await _client.ShippingMethods().GetShippingMethodByIdAsync(_testShippingMethods[0].Id);
            Assert.IsTrue(response.Success);

            ShippingMethod shippingMethod = response.Result;
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
            Response<ShippingMethodQueryResult> response = await _client.ShippingMethods().QueryShippingMethodsAsync();
            Assert.IsTrue(response.Success);

            ShippingMethodQueryResult shippingMethodQueryResult = response.Result;
            Assert.NotNull(shippingMethodQueryResult.Results);
            Assert.GreaterOrEqual(shippingMethodQueryResult.Results.Count, 1);

            int limit = 2;
            response = await _client.ShippingMethods().QueryShippingMethodsAsync(limit: limit);
            Assert.IsTrue(response.Success);

            shippingMethodQueryResult = response.Result;
            Assert.NotNull(shippingMethodQueryResult.Results);
            Assert.LessOrEqual(shippingMethodQueryResult.Results.Count, limit);
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
            Response<ShippingMethod> response = await _client.ShippingMethods().CreateShippingMethodAsync(shippingMethodDraft);
            Assert.IsTrue(response.Success);

            ShippingMethod shippingMethod = response.Result;
            Assert.NotNull(shippingMethod.Id);

            string deletedShippingMethodId = shippingMethod.Id;

            response = await _client.ShippingMethods().DeleteShippingMethodAsync(shippingMethod);
            Assert.IsTrue(response.Success);

            response = await _client.ShippingMethods().GetShippingMethodByIdAsync(deletedShippingMethodId);
            Assert.IsFalse(response.Success);
        }

        /// <summary>
        /// Tests the ShippingMethodManager.UpdateShippingMethodAsync method.
        /// </summary>
        /// <see cref="ShippingMethodManager.UpdateShippingMethodAsync(commercetools.ShippingMethods.ShippingMethod, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdateShippingMethodAsync()
        {
            string newName = string.Concat("Test Shipping Method ", Helper.GetRandomString(10));
            string newDescription = string.Concat("Test Description ", Helper.GetRandomString(10));

            GenericAction changeNameAction = new GenericAction("changeName");
            changeNameAction.SetProperty("name", newName);

            GenericAction setDescriptionAction = new GenericAction("setDescription");
            setDescriptionAction.SetProperty("description", newDescription);

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(changeNameAction);
            actions.Add(setDescriptionAction);

            Response<ShippingMethod> response = await _client.ShippingMethods().UpdateShippingMethodAsync(_testShippingMethods[0], actions);
            Assert.IsTrue(response.Success);

            _testShippingMethods[0] = response.Result;
            Assert.NotNull(_testShippingMethods[0].Id);

            foreach (string language in _project.Languages)
            {
                Assert.AreEqual(_testShippingMethods[0].Name, newName);
                Assert.AreEqual(_testShippingMethods[0].Description, newDescription);
            }
        }
    }
}