using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.ProductTypes;

using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the ProductTypeManager class.
    /// </summary>
    [TestFixture]
    public class ProductTypeManagerTest
    {
        private Client _client;
        private List<ProductType> _testProductTypes;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());
            _testProductTypes = new List<ProductType>();

            for (int i = 0; i < 5; i++)
            {
                ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
                Task<ProductType> task = _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
                task.Wait();

                ProductType productType = task.Result;

                Assert.NotNull(productType.Id);

                _testProductTypes.Add(productType);
            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (ProductType productType in _testProductTypes)
            {
                Task task = _client.ProductTypes().DeleteProductTypeAsync(productType);
                task.Wait();
            }
        }

        /// <summary>
        /// Tests the ProductTypeManager.GetProductTypeByIdAsync method.
        /// </summary>
        /// <see cref="ProductTypeManager.GetProductTypeByIdAsync"/>
        [Test]
        public async Task ShouldGetProductTypeByIdAsync()
        {
            ProductType productType = await _client.ProductTypes().GetProductTypeByIdAsync(_testProductTypes[0].Id);

            Assert.NotNull(productType.Id);
            Assert.AreEqual(productType.Id, _testProductTypes[0].Id);
        }

        /// <summary>
        /// Tests the ProductTypeManager.GetProductTypeByKeyAsync method.
        /// </summary>
        /// <see cref="ProductTypeManager.GetProductTypeByKeyAsync"/>
        [Test]
        public async Task ShouldGetProductTypeByKeyAsync()
        {
            ProductType productType = await _client.ProductTypes().GetProductTypeByKeyAsync(_testProductTypes[0].Key);

            Assert.NotNull(productType.Id);
            Assert.AreEqual(productType.Id, _testProductTypes[0].Id);
        }

        /// <summary>
        /// Tests the ProductTypeManager.QueryProductTypesAsync method.
        /// </summary>
        /// <see cref="ProductTypeManager.QueryProductTypesAsync"/>
        [Test]
        public async Task ShouldQueryProductTypesAsync()
        {
            ProductTypeQueryResult result = await _client.ProductTypes().QueryProductTypesAsync();

            Assert.NotNull(result.Results);
            Assert.GreaterOrEqual(result.Results.Count, 1);

            int limit = 2;
            result = await _client.ProductTypes().QueryProductTypesAsync(limit: limit);

            Assert.NotNull(result.Results);
            Assert.LessOrEqual(result.Results.Count, limit);
        }

        /// <summary>
        /// Tests the ProductTypeManager.CreateProductTypeAsync and ProductTypeManager.DeleteProductTypeAsync methods.
        /// </summary>
        /// <see cref="ProductTypeManager.CreateProductTypeAsync"/>
        /// <seealso cref="ProductTypeManager.DeleteProductTypeAsync"/>
        [Test]
        public async Task ShouldCreateAndDeleteProductTypeAsync()
        {
            ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
            ProductType productType = await _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);

            Assert.NotNull(productType.Id);

            string deletedProductTypeId = productType.Id;

            await _client.ProductTypes().DeleteProductTypeAsync(productType);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate
                {
                    Task task = _client.ProductTypes().GetProductTypeByIdAsync(deletedProductTypeId);
                    task.Wait();
                });
        }

        /// <summary>
        /// Tests the ProductTypeManager.UpdateProductTypeAsync method.
        /// </summary>
        /// <see cref="ProductTypeManager.UpdateProductTypeAsync"/>
        [Test]
        public async Task ShouldUpdateProductTypeAsync()
        {
            List<JObject> actions = new List<JObject>();

            string newKey = Helper.GetRandomString(15);
            string newName = string.Concat("Test Product Type", Helper.GetRandomString(10));

            actions.Add(
                JObject.FromObject(new
                {
                    action = "setKey",
                    key = newKey
                })
            );

            actions.Add(
                JObject.FromObject(new
                {
                    action = "changeName",
                    name = newName
                })
            );

            _testProductTypes[0] = await _client.ProductTypes().UpdateProductTypeAsync(_testProductTypes[0], actions);

            Assert.NotNull(_testProductTypes[0].Id);
            Assert.AreEqual(_testProductTypes[0].Key, newKey);
            Assert.AreEqual(_testProductTypes[0].Name, newName);
        }
    }
}