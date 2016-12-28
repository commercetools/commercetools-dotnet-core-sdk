using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.ProductTypes;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

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
                Task<Response<ProductType>> task = _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
                task.Wait();
                Assert.IsTrue(task.Result.Success);

                ProductType productType = task.Result.Result;
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
            Response<ProductType> response = await _client.ProductTypes().GetProductTypeByIdAsync(_testProductTypes[0].Id);
            Assert.IsTrue(response.Success);

            ProductType productType = response.Result;
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
            Response<ProductType> response = await _client.ProductTypes().GetProductTypeByKeyAsync(_testProductTypes[0].Key);
            Assert.IsTrue(response.Success);

            ProductType productType = response.Result;
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
            Response<ProductTypeQueryResult> response = await _client.ProductTypes().QueryProductTypesAsync();
            Assert.IsTrue(response.Success);

            ProductTypeQueryResult productTypeQueryResult = response.Result;
            Assert.NotNull(productTypeQueryResult.Results);
            Assert.GreaterOrEqual(productTypeQueryResult.Results.Count, 1);

            int limit = 2;
            response = await _client.ProductTypes().QueryProductTypesAsync(limit: limit);
            Assert.IsTrue(response.Success);

            productTypeQueryResult = response.Result;
            Assert.NotNull(productTypeQueryResult.Results);
            Assert.LessOrEqual(productTypeQueryResult.Results.Count, limit);
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
            Response<ProductType> response = await _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
            Assert.IsTrue(response.Success);

            ProductType productType = response.Result;
            Assert.NotNull(productType.Id);

            string deletedProductTypeId = productType.Id;

            Response<JObject> deleteResponse = await _client.ProductTypes().DeleteProductTypeAsync(productType);
            Assert.IsTrue(deleteResponse.Success);

            response = await _client.ProductTypes().GetProductTypeByIdAsync(deletedProductTypeId);
            Assert.IsFalse(response.Success);
        }

        /// <summary>
        /// Tests the ProductTypeManager.UpdateProductTypeAsync method.
        /// </summary>
        /// <see cref="ProductTypeManager.UpdateProductTypeAsync"/>
        [Test]
        public async Task ShouldUpdateProductTypeAsync()
        {
            string newKey = Helper.GetRandomString(15);
            string newName = string.Concat("Test Product Type", Helper.GetRandomString(10));

            GenericAction setKeyAction = new GenericAction("setKey");
            setKeyAction.SetProperty("key", newKey);

            GenericAction changeNameAction = new GenericAction("changeName");
            changeNameAction.SetProperty("name", newName);

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(setKeyAction);
            actions.Add(changeNameAction);

            Response<ProductType> response = await _client.ProductTypes().UpdateProductTypeAsync(_testProductTypes[0], actions);
            Assert.IsTrue(response.Success);

            _testProductTypes[0] = response.Result;
            Assert.NotNull(_testProductTypes[0].Id);
            Assert.AreEqual(_testProductTypes[0].Key, newKey);
            Assert.AreEqual(_testProductTypes[0].Name, newName);
        }
    }
}
