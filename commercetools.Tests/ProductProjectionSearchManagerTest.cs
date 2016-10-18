using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Project;
using commercetools.ProductTypes;
using commercetools.ProductProjections;
using commercetools.ProductProjectionSearch;
using commercetools.Products;
using commercetools.TaxCategories;

using NUnit.Framework;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the ProductProjectionSearchManager class.
    /// </summary>
    [TestFixture]
    public class ProductProjectionSearchManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private ProductType _testProductType;
        private TaxCategory _testTaxCategory;
        private List<Product> _testProducts;

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

            Assert.NotNull(_project);

            ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
            Task<ProductType> testProductTypeTask = _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
            testProductTypeTask.Wait();
            _testProductType = testProductTypeTask.Result;

            Assert.NotNull(_testProductType.Id);

            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<TaxCategory> taxCategoryTask = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            taxCategoryTask.Wait();
            _testTaxCategory = taxCategoryTask.Result;

            Assert.NotNull(_testTaxCategory.Id);

            _testProducts = new List<Product>();

            for (int i = 0; i < 5; i++)
            {
                ProductDraft productDraft = Helper.GetTestProductDraft(_project, _testProductType.Id, _testTaxCategory.Id);
                LocalizedString name = new LocalizedString();

                foreach (string language in _project.Languages)
                {
                    name.SetValue(language, string.Concat("Test Product ", i, " ", language, " ", Helper.GetRandomString(10)));
                }

                productDraft.Name = name;

                Task<Product> productTask = _client.Products().CreateProductAsync(productDraft);
                productTask.Wait();
                Product product = productTask.Result;

                Assert.NotNull(product.Id);

                _testProducts.Add(product);
            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (Product product in _testProducts)
            {
                Task<Product> productTask = _client.Products().DeleteProductAsync(product);
                productTask.Wait();
            }

            Task task = _client.ProductTypes().DeleteProductTypeAsync(_testProductType);
            task.Wait();

            task = _client.TaxCategories().DeleteTaxCategoryAsync(_testTaxCategory);
            task.Wait();
        }

        /// <summary>
        /// Tests the ProductProjectionSearchManager.SearchProductProjectionsAsync method.
        /// </summary>
        /// <see cref="ProductProjectionSearchManager.SearchProductProjectionsAsync"/>
        [Test]
        public async Task ShouldSearchProductProjectionsAsync()
        {
            ProductProjectionQueryResult result = await _client.ProductProjectionSearch().SearchProductProjectionsAsync("Test Product 1");

            Assert.NotNull(result.Results);
            Assert.GreaterOrEqual(result.Results.Count, 1);
        }
    }
}