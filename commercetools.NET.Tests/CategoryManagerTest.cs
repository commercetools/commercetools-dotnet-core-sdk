using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Categories;
using commercetools.Project;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the CategoryManager class.
    /// </summary>
    [TestFixture]
    public class CategoryManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Category> _testCategories;
        
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

            _testCategories = new List<Category>();

            for (int i = 0; i < 5; i++)
            {
                CategoryDraft categoryDraft = Helper.GetTestCategoryDraft(_project);
                Task<Category> categoryTask = _client.Categories().CreateCategoryAsync(categoryDraft);
                categoryTask.Wait();
                Category category = categoryTask.Result;

                Assert.NotNull(category.Id);

                _testCategories.Add(category);
            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (Category category in _testCategories)
            {
                Task<Category> task = _client.Categories().DeleteCategoryAsync(category);
                task.Wait();
            }
        }

        /// <summary>
        /// Tests the CategoryManager.GetCategoryByIdAsync method.
        /// </summary>
        /// <see cref="CategoryManager.GetCategoryByIdAsync"/>
        [Test]
        public async Task ShouldGetCategoryByIdAsync()
        {
            Category category = await _client.Categories().GetCategoryByIdAsync(_testCategories[0].Id);

            Assert.NotNull(category.Id);
            Assert.AreEqual(category.Id, _testCategories[0].Id);
        }

        /// <summary>
        /// Tests the CategoryManager.QueryCategoriesAsync method.
        /// </summary>
        /// <see cref="CategoryManager.QueryCategoriesAsync"/>
        [Test]
        public async Task ShouldQueryCategoriesAsync()
        {
            CategoryQueryResult result = await _client.Categories().QueryCategoriesAsync();

            Assert.NotNull(result.Results);
            Assert.GreaterOrEqual(result.Results.Count, 1);

            int limit = 2;
            result = await _client.Categories().QueryCategoriesAsync(limit: limit);

            Assert.NotNull(result.Results);
            Assert.LessOrEqual(result.Results.Count, limit);

        }

        /// <summary>
        /// Tests the CategoryManager.CreateCategoryAsync and CategoryManager.DeleteCategoryAsync methods.
        /// </summary>
        /// <see cref="CategoryManager.CreateCategoryAsync"/>
        /// <seealso cref="CategoryManager.DeleteCategoryAsync(commercetools.Categories.Category)"/>
        [Test]
        public async Task ShouldCreateAndDeleteCategoryAsync()
        {
            CategoryDraft categoryDraft = Helper.GetTestCategoryDraft(_project);
            Category category = await _client.Categories().CreateCategoryAsync(categoryDraft);

            Assert.NotNull(category.Id);

            string deletedCategoryId = category.Id;

            category = await _client.Categories().DeleteCategoryAsync(category);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate
                {
                    Task<Category> task = _client.Categories().GetCategoryByIdAsync(deletedCategoryId);
                    task.Wait();
                });
        }

        /// <summary>
        /// Tests the CategoryManager.UpdateCategoryAsync method.
        /// </summary>
        /// <see cref="CategoryManager.UpdateCategoryAsync(commercetools.Categories.Category, System.Collections.Generic.List{Newtonsoft.Json.Linq.JObject})"/>
        [Test]
        public async Task ShouldUpdateCategoryAsync()
        {
            List<JObject> actions = new List<JObject>();

            LocalizedString newName = new LocalizedString();
            LocalizedString newSlug = new LocalizedString();

            foreach (string language in _project.Languages)
            {
                newName.SetValue(language, string.Concat("New Name ", language));
                newSlug.SetValue(language, string.Concat("slug-updated-", language));
            }

            JObject changeNameAction = JObject.FromObject(new
            {
                action = "changeName",
                name = newName,
                staged = true
            });

            JObject changeSlugAction = JObject.FromObject(new
            {
                action = "changeSlug",
                slug = newSlug,
                staged = true
            });

            actions.Add(changeNameAction);
            actions.Add(changeSlugAction);

            _testCategories[2] = await _client.Categories().UpdateCategoryAsync(_testCategories[2], actions);

            Assert.NotNull(_testCategories[2].Id);

            foreach (string language in _project.Languages)
            {
                Assert.AreEqual(_testCategories[2].Name.GetValue(language), newName.GetValue(language));
                Assert.AreEqual(_testCategories[2].Slug.GetValue(language), newSlug.GetValue(language));
            }
        }
    }
}