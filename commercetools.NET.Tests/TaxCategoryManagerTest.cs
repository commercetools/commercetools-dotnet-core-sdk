using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Project;
using commercetools.TaxCategories;

using NUnit.Framework;

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

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            Assert.NotNull(_project.Countries);
            Assert.GreaterOrEqual(_project.Countries.Count, 1);

            _testTaxCategories = new List<TaxCategory>();

            for (int i = 0; i < 5; i++)
            {
                TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
                Task<Response<TaxCategory>> taxCategoryTask = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
                taxCategoryTask.Wait();
                Assert.IsTrue(taxCategoryTask.Result.Success);

                TaxCategory taxCategory = taxCategoryTask.Result.Result;
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
            Response<TaxCategory> response = await _client.TaxCategories().GetTaxCategoryByIdAsync(_testTaxCategories[0].Id);
            Assert.IsTrue(response.Success);

            TaxCategory taxCategory = response.Result;
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
            Response<TaxCategoryQueryResult> response = await _client.TaxCategories().QueryTaxCategoriesAsync();
            Assert.IsTrue(response.Success);

            TaxCategoryQueryResult taxCategoryQueryResult = response.Result;
            Assert.NotNull(taxCategoryQueryResult.Results);
            Assert.GreaterOrEqual(taxCategoryQueryResult.Results.Count, 1);

            int limit = 2;
            response = await _client.TaxCategories().QueryTaxCategoriesAsync(limit: limit);

            taxCategoryQueryResult = response.Result;
            Assert.NotNull(taxCategoryQueryResult.Results);
            Assert.LessOrEqual(taxCategoryQueryResult.Results.Count, limit);
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
            Response<TaxCategory> response = await _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            Assert.IsTrue(response.Success);

            TaxCategory taxCategory = response.Result;
            Assert.NotNull(taxCategory.Id);
            Assert.AreEqual(taxCategory.Name, taxCategoryDraft.Name);
            Assert.AreEqual(taxCategory.Description, taxCategoryDraft.Description);
            Assert.AreEqual(taxCategory.Rates.Count, taxCategoryDraft.Rates.Count);

            string deletedTaxCategoryId = taxCategory.Id;

            Response<JObject> deleteResponse = await _client.TaxCategories().DeleteTaxCategoryAsync(taxCategory.Id, taxCategory.Version);
            Assert.IsTrue(deleteResponse.Success);

            response = await _client.TaxCategories().GetTaxCategoryByIdAsync(deletedTaxCategoryId);
            Assert.IsFalse(response.Success);
        }

        /// <summary>
        /// Tests the TaxCategoryManager.UpdateTaxCategoryAsync method.
        /// </summary>
        /// <see cref="TaxCategoryManager.UpdateTaxCategoryAsync(commercetools.TaxCategories.TaxCategory, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdateTaxCategoryAsync()
        {
            string newName = string.Concat(_testTaxCategories[1].Name, " Updated");
            string newDescription = string.Concat(_testTaxCategories[1].Description, " Updated");

            GenericAction changeNameAction = new GenericAction("changeName");
            changeNameAction.SetProperty("name", newName);

            GenericAction setDescriptionAction = new GenericAction("setDescription");
            changeNameAction.SetProperty("description", newDescription);

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(changeNameAction);
            actions.Add(setDescriptionAction);

            Response<TaxCategory> response = await _client.TaxCategories().UpdateTaxCategoryAsync(_testTaxCategories[1], actions);
            Assert.IsTrue(response.Success);

            _testTaxCategories[1] = response.Result;
            Assert.NotNull(_testTaxCategories[1].Id);
            Assert.AreEqual(_testTaxCategories[1].Name, newName);
            Assert.AreEqual(_testTaxCategories[1].Description, newDescription);
        }
    }
}
