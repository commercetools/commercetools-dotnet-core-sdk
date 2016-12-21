using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Project;
using commercetools.ProductProjections;
using commercetools.Products;

using NUnit.Framework;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the ProductProjectionManager class.
    /// </summary>
    [TestFixture]
    public class ProductProjectionManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Product> _products;

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

            _products = new List<Product>();

            Task<Response<ProductQueryResult>> productQueryTask = _client.Products().QueryProductsAsync();
            productQueryTask.Wait();
            Assert.IsTrue(productQueryTask.Result.Success);

            ProductQueryResult productQueryResult = productQueryTask.Result.Result;
            Assert.NotNull(productQueryResult.Results);
            Assert.GreaterOrEqual(productQueryResult.Results.Count, 1);

            _products.AddRange(productQueryResult.Results);
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
        }

        /// <summary>
        /// Tests the ProductProjectionManager.GetProductProjectionByIdAsync method.
        /// </summary>
        /// <see cref="ProductProjectionManager.GetProductProjectionByIdAsync"/>
        [Test]
        public async Task ShouldGetProductProjectionByIdAsync()
        {
            Response<ProductProjection> response = await _client.ProductProjections().GetProductProjectionByIdAsync(_products[0].Id);
            Assert.IsTrue(response.Success);

            ProductProjection productProjection = response.Result;
            Assert.NotNull(productProjection.Id);
            Assert.AreEqual(productProjection.Id, _products[0].Id);
        }

        /// <summary>
        /// Tests the ProductProjectionManager.GetProductProjectionByKeyAsync method.
        /// </summary>
        /// <see cref="ProductProjectionManager.GetProductProjectionByKeyAsync"/>
        [Test]
        public async Task ShouldGetProductProjectionByKeyAsync()
        {
            List<Product> productsWithKey = _products.Where(p => !string.IsNullOrWhiteSpace(p.Key)).ToList();
            Assert.GreaterOrEqual(productsWithKey.Count, 1);

            Response<ProductProjection> response = await _client.ProductProjections().GetProductProjectionByKeyAsync(_products[1].Key);
            Assert.IsTrue(response.Success);

            ProductProjection productProjection = response.Result;
            Assert.NotNull(productProjection.Id);
            Assert.AreEqual(productProjection.Id, _products[1].Id);
        }

        /// <summary>
        /// Tests the ProductProjectionManager.QueryProductProjectionsAsync method.
        /// </summary>
        /// <see cref="ProductProjectionManager.QueryProductProjectionsAsync"/>
        [Test]
        public async Task ShouldQueryProductProjectionsAsync()
        {
            Response<ProductProjectionQueryResult> response = await _client.ProductProjections().QueryProductProjectionsAsync();
            Assert.IsTrue(response.Success);

            ProductProjectionQueryResult productProjectionQueryResult = response.Result;
            Assert.NotNull(productProjectionQueryResult.Results);

            int limit = 2;
            response = await _client.ProductProjections().QueryProductProjectionsAsync(limit: limit);
            Assert.IsTrue(response.Success);

            productProjectionQueryResult = response.Result;
            Assert.NotNull(productProjectionQueryResult.Results);
            Assert.LessOrEqual(productProjectionQueryResult.Results.Count, limit);
        }
    }
}
