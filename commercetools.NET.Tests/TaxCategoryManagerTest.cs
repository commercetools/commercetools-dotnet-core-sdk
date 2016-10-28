using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Carts;
using commercetools.Customers;
using commercetools.Messages;
using commercetools.Products;
using commercetools.ProductTypes;
using commercetools.Project;
using commercetools.ShippingMethods;
using commercetools.TaxCategories;

using NUnit.Framework;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the TaxCategoryManager class.
    /// </summary>
    [TestFixture]
    public class TaxCategoryManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<TaxCategory> _testTaxCategories;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Project.Project> project = _client.Project().GetProjectAsync();
            project.Wait();
            _project = project.Result;

            Assert.NotNull(_project.Countries);
            Assert.GreaterOrEqual(_project.Countries.Count, 1);

            _testTaxCategories = new List<TaxCategory>();

            for (int i = 0; i < 5; i++)
            {
                TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
                Task<TaxCategory> taxCategoryTask = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
                taxCategoryTask.Wait();
                TaxCategory taxCategory = taxCategoryTask.Result;

                Assert.NotNull(taxCategory.Id);

                _testTaxCategories.Add(taxCategory);
            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (TaxCategory taxCategory in _testTaxCategories)
            {
                Task task = _client.TaxCategories().DeleteTaxCategoryAsync(taxCategory);
                task.Wait();
            }
        }

        /// <summary>
        /// Tests the TaxCategoryManager.GetTaxCategoryByIdAsync method.
        /// </summary>
        /// <see cref="TaxCategoryManager.GetTaxCategoryByIdAsync"/>
        [Test]
        public async Task ShouldGetTaxCategoryByIdAsync()
        {
            TaxCategory taxCategory = await _client.TaxCategories().GetTaxCategoryByIdAsync(_testTaxCategories[0].Id);

            Assert.NotNull(taxCategory.Id);
            Assert.AreEqual(taxCategory.Id, _testTaxCategories[0].Id);
        }

        /// <summary>
        /// Tests the TaxCategoryManager.QueryTaxCategoriesAsync method.
        /// </summary>
        /// <see cref="TaxCategoryManager.QueryTaxCategoriesAsync"/>
        [Test]
        public async Task ShouldQueryTaxCategoriesAsync()
        {
            TaxCategoryQueryResult result = await _client.TaxCategories().QueryTaxCategoriesAsync();

            Assert.NotNull(result.Results);
            Assert.GreaterOrEqual(result.Results.Count, 1);

            int limit = 2;
            result = await _client.TaxCategories().QueryTaxCategoriesAsync(limit: limit);

            Assert.NotNull(result.Results);
            Assert.LessOrEqual(result.Results.Count, limit);
        }

        /// <summary>
        /// Tests the TaxCategoryManager.CreateTaxCategoryAsync and TaxCategoryManager.DeleteTaxCategoryAsync methods.
        /// </summary>
        /// <see cref="TaxCategoryManager.CreateTaxCategoryAsync"/>
        /// <seealso cref="TaxCategoryManager.DeleteTaxCategoryAsync(commercetools.TaxCategories.TaxCategory)"/>
        [Test]
        public async Task ShouldCreateAndDeleteTaxCategoryAsync()
        {
            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);

            TaxCategory taxCategory = await _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);

            Assert.NotNull(taxCategory.Id);
            Assert.AreEqual(taxCategory.Name, taxCategoryDraft.Name);
            Assert.AreEqual(taxCategory.Description, taxCategoryDraft.Description);
            Assert.AreEqual(taxCategory.Rates.Count, taxCategoryDraft.Rates.Count);

            string deletedTaxCategoryId = taxCategory.Id;

            await _client.TaxCategories().DeleteTaxCategoryAsync(taxCategory.Id, taxCategory.Version);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate 
                {
                    Task task = _client.TaxCategories().GetTaxCategoryByIdAsync(deletedTaxCategoryId);
                    task.Wait();
                });
        }

        /// <summary>
        /// Tests the TaxCategoryManager.UpdateTaxCategoryAsync method.
        /// </summary>
        /// <see cref="TaxCategoryManager.UpdateTaxCategoryAsync(commercetools.TaxCategories.TaxCategory, System.Collections.Generic.List{Newtonsoft.Json.Linq.JObject})"/>
        [Test]
        public async Task ShouldUpdateTaxCategoryAsync()
        {
            List<JObject> actions = new List<JObject>();

            string newName = string.Concat(_testTaxCategories[1].Name, " Updated");
            string newDescription = string.Concat(_testTaxCategories[1].Description, " Updated");

            JObject changeNameAction = JObject.FromObject(new
            {
                action = "changeName",
                name = newName
            });

            JObject setDescriptionAction = JObject.FromObject(new
            {
                action = "setDescription",
                description = newDescription
            });

            actions.Add(changeNameAction);
            actions.Add(setDescriptionAction);

            _testTaxCategories[1] = await _client.TaxCategories().UpdateTaxCategoryAsync(_testTaxCategories[1], actions);

            Assert.NotNull(_testTaxCategories[1].Id);
            Assert.AreEqual(_testTaxCategories[1].Name, newName);
            Assert.AreEqual(_testTaxCategories[1].Description, newDescription);
        }
    }
}
