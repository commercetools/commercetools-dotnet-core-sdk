using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Messages;
using System.Linq;
using NUnit.Framework;
using commercetools.Products;
using commercetools.Project;
using commercetools.ProductTypes;
using commercetools.TaxCategories;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the CustomerManager class.
    /// </summary>
    [TestFixture]
    public class MessageManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private string _testProductId;
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

            ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
            Task<Response<ProductType>> productTypeTask = _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
            productTypeTask.Wait();
            Assert.IsTrue(productTypeTask.Result.Success);

            ProductType testProductType = productTypeTask.Result.Result;
            Assert.NotNull(testProductType.Id);

            TaxCategoryDraft testTaxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<Response<TaxCategory>> taxCategoryTask = _client.TaxCategories().CreateTaxCategoryAsync(testTaxCategoryDraft);
            taxCategoryTask.Wait();
            Assert.IsTrue(taxCategoryTask.Result.Success);

            ProductDraft productDraft = Helper.GetTestProductDraft(_project, testProductType.Id, taxCategoryTask.Result.Result.Id);
            Task<Response<Product>> productTask = _client.Products().CreateProductAsync(productDraft);
            productTask.Wait();
            Assert.IsTrue(productTask.Result.Success);

            Product testProduct = productTask.Result.Result;
            Assert.NotNull(testProduct.Id);
            _testProductId = testProduct.Id;

            var deleteProduct = _client.Products().DeleteProductAsync(testProduct);
            deleteProduct.Wait();
            Assert.IsTrue(deleteProduct.Result.Success);

            var deleteProductType = _client.ProductTypes().DeleteProductTypeAsync(testProductType);
            deleteProductType.Wait();
            Assert.IsTrue(deleteProductType.Result.Success);

            var deleteTaxCategory = _client.TaxCategories().DeleteTaxCategoryAsync(taxCategoryTask.Result.Result);
            deleteTaxCategory.Wait();
            Assert.IsTrue(deleteTaxCategory.Result.Success);
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
        }

        /// <summary>
        /// Tests the ProjectManager.GetProjectAsync method.
        /// </summary>
        /// <see cref="MessageManager.QueryMessagesAsync"/>
        [Test]
        public async Task ShouldGetCreatedAndDeletedProductMessage()
        {
            MessageManager messageManager = new MessageManager(_client);
            //var result = await messageManager.QueryMessagesAsync(null, "createdAt desc");
            var result = await messageManager.QueryMessagesAsync(string.Format("resource(id=\"{0}\") and resource(typeId=\"product\")", _testProductId));
            Assert.NotNull(result);
            var messages = result.Result;
            Assert.NotNull(messages);
            Assert.NotNull(messages.Results);
            Assert.IsTrue(messages.Results.Count == 2);
            Assert.IsFalse(messages.Results.Any(m => string.IsNullOrEmpty(m.Type)));
            Assert.IsTrue(messages.Results.Count(m => m.Type.Equals("ProductCreated")) == 1);
            Assert.IsTrue(messages.Results.Count(m => m.Type.Equals("ProductDeleted")) == 1);

            ProductCreatedMessage productCreatedMessage = messages.Results.First(m => m.Type.Equals("ProductCreated")) as ProductCreatedMessage;
            Assert.NotNull(productCreatedMessage);
            Assert.NotNull(productCreatedMessage.ProductProjection);

            ProductDeletedMessage productDeletedMessage = messages.Results.First(m => m.Type.Equals("ProductDeleted")) as ProductDeletedMessage;
            Assert.NotNull(productDeletedMessage);
            Assert.NotNull(productDeletedMessage.CurrentProjection);
            Assert.NotNull(productDeletedMessage.RemovedImageUrls);
        }

    }
}