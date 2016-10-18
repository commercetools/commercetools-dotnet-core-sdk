using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Products;
using commercetools.ProductTypes;
using commercetools.Project;
using commercetools.TaxCategories;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the ProductManager class.
    /// </summary>
    [TestFixture]
    public class ProductManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Product> _testProducts;
        private ProductType _testProductType;
        private TaxCategory _testTaxCategory;

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
        /// Tests the ProductManager.GetProductByIdAsync method.
        /// </summary>
        /// <see cref="ProductManager.GetProductByIdAsync"/>
        [Test]
        public async Task ShouldGetProductByIdAsync()
        {
            Product product = await _client.Products().GetProductByIdAsync(_testProducts[0].Id);

            Assert.NotNull(product.Id);
            Assert.AreEqual(product.Id, _testProducts[0].Id);
        }

        /// <summary>
        /// Tests the ProductManager.GetProductByKeyAsync method.
        /// </summary>
        /// <see cref="ProductManager.GetProductByKeyAsync"/>
        [Test]
        public async Task ShouldGetProductByKeyAsync()
        {
            Product product = await _client.Products().GetProductByKeyAsync(_testProducts[1].Key);

            Assert.NotNull(product.Id);
            Assert.AreEqual(product.Key, _testProducts[1].Key);
        }

        /// <summary>
        /// Tests the ProductManager.QueryProductsAsync method.
        /// </summary>
        /// <see cref="ProductManager.QueryProductsAsync"/>
        [Test]
        public async Task ShouldQueryProductsAsync()
        {
            ProductQueryResult result = await _client.Products().QueryProductsAsync();

            Assert.NotNull(result.Results);
            Assert.GreaterOrEqual(result.Results.Count, 1);

            int limit = 2;
            result = await _client.Products().QueryProductsAsync(limit: limit);

            Assert.NotNull(result.Results);
            Assert.LessOrEqual(result.Results.Count, limit);
        }

        /// <summary>
        /// Tests the ProductManager.CreateProductAsync and ProductManager.DeleteProductAsync methods.
        /// </summary>
        /// <see cref="ProductManager.CreateProductAsync"/>
        /// <seealso cref="ProductManager.DeleteProductAsync(commercetools.Products.Product)"/>
        [Test]
        public async Task ShouldCreateAndDeleteProductAsync()
        {
            ProductDraft productDraft = Helper.GetTestProductDraft(_project, _testProductType.Id, _testTaxCategory.Id);

            LocalizedString name = new LocalizedString();
            LocalizedString slug = new LocalizedString();

            foreach (string language in _project.Languages)
            {
                name.SetValue(language, string.Concat("Test Product", language, " ", Helper.GetRandomString(10)));
                slug.SetValue(language, string.Concat("test-product-", language, "-", Helper.GetRandomString(10)));
            }

            productDraft.Name = name;
            productDraft.Slug = slug;

            Product product = await _client.Products().CreateProductAsync(productDraft);

            Assert.NotNull(product.Id);

            string deletedProductId = product.Id;

            product = await _client.Products().DeleteProductAsync(product.Id, product.Version);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate
                {
                    Task task = _client.Products().GetProductByIdAsync(deletedProductId);
                    task.Wait();
                });
        }

        /// <summary>
        /// Tests the ProductManager.UpdateProductAsync method.
        /// </summary>
        /// <see cref="ProductManager.UpdateProductAsync"/>
        [Test]
        public async Task ShouldUpdateProductAsync()
        {
            List<JObject> actions = new List<JObject>();

            string newKey = Helper.GetRandomString(15);
            LocalizedString newSlug = new LocalizedString();

            foreach (string language in _project.Languages)
            {
                newSlug.SetValue(language, string.Concat("updated-product-", language, "-", Helper.GetRandomString(10)));
            }

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
                    action = "changeSlug",
                    slug = newSlug,
                    staged = true
                })
            );

            _testProducts[2] = await _client.Products().UpdateProductAsync(_testProducts[2], actions);

            Assert.NotNull(_testProducts[2].Id);

            Assert.AreEqual(_testProducts[2].Key, newKey);

            foreach (string language in _project.Languages)
            {
                Assert.AreEqual(_testProducts[2].MasterData.Staged.Slug.GetValue(language), newSlug.GetValue(language));
            }
        }

        /// <summary>
        /// Tests the ProductManager.UpdateProductByKeyAsync method.
        /// </summary>
        /// <see cref="ProductManager.UpdateProductByKeyAsync"/>
        [Test]
        public async Task ShouldUpdateProductByKeyAsync()
        {
            List<JObject> actions = new List<JObject>();

            LocalizedString newName = new LocalizedString();
            LocalizedString newSlug = new LocalizedString();

            foreach (string language in _project.Languages)
            {
                newName.SetValue(language, string.Concat("Updated Product ", language, " ", Helper.GetRandomString(10)));
                newSlug.SetValue(language, string.Concat("updated-product-", language, "-", Helper.GetRandomString(10)));
            }

            actions.Add(
                JObject.FromObject(new
                {
                    action = "changeName",
                    name = newName,
                    staged = true
                })
            );

            actions.Add(
                JObject.FromObject(new
                {
                    action = "changeSlug",
                    slug = newSlug,
                    staged = true
                })
            );

            _testProducts[1] = await _client.Products().UpdateProductByKeyAsync(_testProducts[1].Key, _testProducts[1].Version, actions);

            Assert.NotNull(_testProducts[1].Id);

            foreach (string language in _project.Languages)
            {
                Assert.AreEqual(_testProducts[1].MasterData.Staged.Name.GetValue(language), newName.GetValue(language));
                Assert.AreEqual(_testProducts[1].MasterData.Staged.Slug.GetValue(language), newSlug.GetValue(language));
            }
        }
    }
}