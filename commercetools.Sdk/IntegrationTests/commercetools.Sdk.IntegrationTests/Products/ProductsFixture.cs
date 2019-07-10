using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.UpdateActions;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using static commercetools.Sdk.IntegrationTests.ProductTypes.ProductTypesFixture;

namespace commercetools.Sdk.IntegrationTests.Products
{
    public static class ProductsFixture
    {
        #region DraftBuilds

        public static ProductDraft DefaultProductDraft(ProductDraft productDraft, ProductType productType)
        {
            var randomInt = TestingUtility.RandomInt(1, 100);
            productDraft.Name = new LocalizedString {{"en", $"Name_{randomInt}"}};
            productDraft.Slug = new LocalizedString {{"en", $"Slug_{randomInt}"}};
            productDraft.ProductType = new ResourceIdentifier<ProductType>
            {
                Key = productType.Key
            };

            productDraft.Key = $"Key_{randomInt}";
            productDraft.Publish = true;
            return productDraft;
        }

        #endregion

        #region WithProduct

        public static async Task WithProduct( IClient client, Action<Product> func)
        {
            await WithProductType(client, async productType =>
                {
                    await With(client, new ProductDraft(),
                        productDraft => DefaultProductDraft(productDraft, productType), func, null, DeleteProduct);
                });
        }
        public static async Task WithProduct( IClient client, Func<ProductDraft, ProductDraft> draftAction, Action<Product> func)
        {
            await With(client, new ProductDraft(), draftAction, func, null, DeleteProduct);
        }

        public static async Task WithProduct( IClient client, Func<Product, Task> func)
        {
            await WithProductType(client, async productType =>
            {
                await WithAsync(client, new ProductDraft(),
                    productDraft => DefaultProductDraft(productDraft, productType), func, null, DeleteProduct);
            });
        }
        public static async Task WithProduct( IClient client, Func<ProductDraft, ProductDraft> draftAction, Func<Product, Task> func)
        {
            await WithAsync(client, new ProductDraft(), draftAction, func, null, DeleteProduct);
        }
        #endregion

        #region WithUpdateableProduct

        public static async Task WithUpdateableProduct(IClient client, Func<Product, Product> func)
        {
            await WithProductType(client, async productType =>
            {
                await WithUpdateable(client, new ProductDraft(),
                    productDraft => DefaultProductDraft(productDraft, productType), func, null, DeleteProduct);
            });
        }

        public static async Task WithUpdateableProduct(IClient client, Func<ProductDraft, ProductDraft> draftAction, Func<Product, Product> func)
        {
            await WithUpdateable(client, new ProductDraft(), draftAction, func, null, DeleteProduct);
        }

        public static async Task WithUpdateableProduct(IClient client, Func<Product, Task<Product>> func)
        {
            await WithProductType(client, async productType =>
            {
                await WithUpdateableAsync(client, new ProductDraft(),
                    productDraft => DefaultProductDraft(productDraft, productType), func, null, DeleteProduct);
            });
        }
        public static async Task WithUpdateableProduct(IClient client, Func<ProductDraft, ProductDraft> draftAction, Func<Product, Task<Product>> func)
        {
            await WithUpdateableAsync(client, new ProductDraft(), draftAction, func, null, DeleteProduct);
        }

        #endregion


        public static async Task DeleteProduct(IClient client, Product product)
        {
            var toBeDeleted = product;
            if (product.MasterData.Published)
            {
                toBeDeleted = await Unpublish(client, product);
            }

            await DeleteResource(client, toBeDeleted);
        }

        public static async Task<Product> Unpublish(IClient client, Product product)
        {
            var updateActions = new List<UpdateAction<Product>>
            {
                new UnpublishUpdateAction()
            };
            var retrievedProduct = await client.ExecuteAsync(new UpdateByIdCommand<Product>(product, updateActions));
            return retrievedProduct;
        }
    }
}
