using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Categories;
using commercetools.Categories.UpdateActions;
using commercetools.Project;

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

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            _testCategories = new List<Category>();

            for (int i = 0; i < 5; i++)
            {
                CategoryDraft categoryDraft = Helper.GetTestCategoryDraft(_project);
                Task<Response<Category>> categoryTask = _client.Categories().CreateCategoryAsync(categoryDraft);
                categoryTask.Wait();
                Assert.IsTrue(categoryTask.Result.Success);

                Category category = categoryTask.Result.Result;
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
                Task<Response<Category>> categoryTask = _client.Categories().DeleteCategoryAsync(category);
                categoryTask.Wait();
            }
        }

        /// <summary>
        /// Tests the CategoryManager.GetCategoryByIdAsync method.
        /// </summary>
        /// <see cref="CategoryManager.GetCategoryByIdAsync"/>
        [Test]
        public async Task ShouldGetCategoryByIdAsync()
        {
            Response<Category> response = await _client.Categories().GetCategoryByIdAsync(_testCategories[0].Id);
            Assert.IsTrue(response.Success);

            Category category = response.Result;
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
            Response<CategoryQueryResult> response = await _client.Categories().QueryCategoriesAsync();
            Assert.IsTrue(response.Success);

            CategoryQueryResult categoryQueryResult = response.Result;
            Assert.NotNull(categoryQueryResult.Results);
            Assert.GreaterOrEqual(categoryQueryResult.Results.Count, 1);

            int limit = 2;
            response = await _client.Categories().QueryCategoriesAsync(limit: limit);
            Assert.IsTrue(response.Success);

            categoryQueryResult = response.Result;
            Assert.NotNull(categoryQueryResult.Results);
            Assert.LessOrEqual(categoryQueryResult.Results.Count, limit);
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
            Response<Category> response = await _client.Categories().CreateCategoryAsync(categoryDraft);
            Assert.IsTrue(response.Success);

            Category category = response.Result;
            Assert.NotNull(category.Id);

            string deletedCategoryId = category.Id;

            response = await _client.Categories().DeleteCategoryAsync(category);
            Assert.IsTrue(response.Success);

            response = await _client.Categories().GetCategoryByIdAsync(deletedCategoryId);
            Assert.IsFalse(response.Success);
        }

        /// <summary>
        /// Tests the CategoryManager.UpdateCategoryAsync method.
        /// </summary>
        /// <see cref="CategoryManager.UpdateCategoryAsync(commercetools.Categories.Category, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdateCategoryAsync()
        {
            LocalizedString newName = new LocalizedString();
            LocalizedString newSlug = new LocalizedString();

            foreach (string language in _project.Languages)
            {
                newName.SetValue(language, string.Concat("New Name ", language));
                newSlug.SetValue(language, string.Concat("slug-updated-", language));
            }

            ChangeNameAction changeNameAction = new ChangeNameAction(newName);

            GenericAction changeSlugAction = new GenericAction("changeSlug");
            changeSlugAction.SetProperty("slug", newSlug);

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(changeNameAction);
            actions.Add(changeSlugAction);

            Response<Category> response = await _client.Categories().UpdateCategoryAsync(_testCategories[2], actions);
            Assert.IsTrue(response.Success);

            _testCategories[2] = response.Result;
            Assert.NotNull(_testCategories[2].Id);

            foreach (string language in _project.Languages)
            {
                Assert.AreEqual(_testCategories[2].Name.GetValue(language), newName.GetValue(language));
                Assert.AreEqual(_testCategories[2].Slug.GetValue(language), newSlug.GetValue(language));
            }
        }
    }
}