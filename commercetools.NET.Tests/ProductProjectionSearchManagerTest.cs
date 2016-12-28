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

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            Assert.NotNull(_project);

            ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
            Task<Response<ProductType>> productTypeTask = _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
            productTypeTask.Wait();
            Assert.IsTrue(productTypeTask.Result.Success);

            _testProductType = productTypeTask.Result.Result;
            Assert.NotNull(_testProductType.Id);

            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<Response<TaxCategory>> taxCategoryTask = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            taxCategoryTask.Wait();
            Assert.IsTrue(taxCategoryTask.Result.Success);

            _testTaxCategory = taxCategoryTask.Result.Result;
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

                Task<Response<Product>> productTask = _client.Products().CreateProductAsync(productDraft);
                productTask.Wait();
                Assert.IsTrue(productTask.Result.Success);

                Product product = productTask.Result.Result;
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
            Task task;

            foreach (Product product in _testProducts)
            {
                task = _client.Products().DeleteProductAsync(product);
                task.Wait();
            }

            task = _client.ProductTypes().DeleteProductTypeAsync(_testProductType);
            task.Wait();

            task = _client.TaxCategories().DeleteTaxCategoryAsync(_testTaxCategory);
            task.Wait();
        }

        /// <summary>
        /// Tests the ProductProjectionSearchManager.SearchProductProjectionsAsync method, retrieving results.
        /// </summary>
        /// <see cref="ProductProjectionSearchManager.SearchProductProjectionsAsync"/>
        [Test]
        public async Task ShouldGetSearchResultsAsync()
        {
            Response<ProductProjectionQueryResult> response 
                = await _client.ProductProjectionSearch().SearchProductProjectionsAsync("Test Product 1", staged: true);
            Assert.IsTrue(response.Success);

            ProductProjectionQueryResult productProjectionQueryResult = response.Result;
            Assert.NotNull(productProjectionQueryResult.Results);
            Assert.GreaterOrEqual(productProjectionQueryResult.Results.Count, 1);
        }

        /// <summary>
        /// Tests the ProductProjectionSearchManager.SearchProductProjectionsAsync method, retrieving facets with results.
        /// </summary>
        /// <see cref="ProductProjectionSearchManager.SearchProductProjectionsAsync"/>
        [Test]
        public async Task ShouldGetSearchResultsAndFacetsAsync()
        {
            string[] facet = new string[] {
                "variants.price.centAmount:range(* to 50),(50 to 100),(100 to *)"
            };

            Response<ProductProjectionQueryResult> response 
                = await _client.ProductProjectionSearch().SearchProductProjectionsAsync("Test Product 1", staged: true, facet: facet);
            Assert.IsTrue(response.Success);

            ProductProjectionQueryResult productProjectionQueryResult = response.Result;
            Assert.NotNull(productProjectionQueryResult.Results);
            Assert.GreaterOrEqual(productProjectionQueryResult.Results.Count, 1);
            Assert.GreaterOrEqual(productProjectionQueryResult.Facets.Count, 1);
        }
    }
}
