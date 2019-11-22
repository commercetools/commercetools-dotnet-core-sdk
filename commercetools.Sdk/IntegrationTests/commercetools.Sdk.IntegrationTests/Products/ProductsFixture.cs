using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Products.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using static commercetools.Sdk.IntegrationTests.ProductTypes.ProductTypesFixture;

namespace commercetools.Sdk.IntegrationTests.Products
{
    public static class ProductsFixture
    {
        #region DraftBuilds

        public static ProductDraft DefaultProductDraft(ProductDraft productDraft)
        {
            var randomInt = TestingUtility.RandomInt();
            productDraft.Name = new LocalizedString {{"en", $"Name_{randomInt}"}};
            productDraft.Description = new LocalizedString {{"en", $"Description_{randomInt}"}};
            productDraft.Slug = new LocalizedString {{"en", $"Slug_{randomInt}"}};
            productDraft.Key = $"Key_{randomInt}";
            productDraft.Publish = true;
            
            var priceCentAmount = TestingUtility.RandomInt(1000, 5000);
            var productPrice = TestingUtility.GetPriceDraft(priceCentAmount);
           
            productDraft.MasterVariant = new ProductVariantDraft
            {
                Key = $"MasterVariant_key_{randomInt}",
                Sku = $"MasterVariant_Sku_{randomInt}",
                Prices = new List<PriceDraft> { productPrice }
            };
            return productDraft;
        }

        public static ProductDraft DefaultProductDraftWithKey(ProductDraft draft, string key)
        {
            var productDraft = DefaultProductDraft(draft);
            productDraft.Key = key;
            return productDraft;
        }
        
        public static ProductDraft DefaultProductDraftWithImages(ProductDraft draft, List<Image> images)
        {
            var productDraft = DefaultProductDraft(draft);
            productDraft.MasterVariant.Images = images;
            return productDraft;
        }
        
        public static ProductDraft DefaultProductDraftWithAssets(ProductDraft draft, List<AssetDraft> assets)
        {
            var productDraft = DefaultProductDraft(draft);
            productDraft.MasterVariant.Assets = assets;
            return productDraft;
        }
        
        public static ProductDraft DefaultProductDraftWithCategory(ProductDraft draft,Category category)
        {
            var productDraft = DefaultProductDraft(draft);
            productDraft.Categories = new List<IReference<Category>>()
            {
                category.ToKeyResourceIdentifier()
            };
            return productDraft;
        }
        public static ProductDraft DefaultProductDraftWithMultipleVariants(ProductDraft draft, int variantsCount = 1)
        {
            var randomInt = TestingUtility.RandomInt();
            var productDraft = DefaultProductDraft(draft);

            var variants = new List<ProductVariantDraft>();
            for (int i = 1; i <= variantsCount; i++)
            {
                var productVariant = new ProductVariantDraft
                {
                    Key = $"variant{i}_Key_{randomInt}",
                    Sku = $"variant{i}_Sku_{randomInt}"
                };
                variants.Add(productVariant);
            }

            productDraft.Variants = variants;
            return productDraft;
        }

        #endregion

        #region WithProduct

        public static async Task WithProduct( IClient client, Action<Product> func)
        {
            await WithProductType(client, async productType =>
            {
                var productDraftWithProductType = new ProductDraft
                {
                    ProductType = productType.ToKeyResourceIdentifier()
                };
                await With(client, productDraftWithProductType,
                    DefaultProductDraft, func, null, DeleteProduct);
            });
        }
        public static async Task WithProduct( IClient client, Func<ProductDraft, ProductDraft> draftAction, Action<Product> func)
        {
            await WithProductType(client, async productType =>
            {
                var productDraftWithProductType = new ProductDraft
                {
                    ProductType = productType.ToKeyResourceIdentifier()
                };

                await With(client, productDraftWithProductType, draftAction, func, null, DeleteProduct);
            });
        }

        public static async Task WithProduct( IClient client, Func<Product, Task> func)
        {
            await WithProductType(client, async productType =>
            {
                var productDraftWithProductType = new ProductDraft
                {
                    ProductType = productType.ToKeyResourceIdentifier()
                };
                await WithAsync(client, productDraftWithProductType,
                    DefaultProductDraft, func, null, DeleteProduct);
            });
        }
        public static async Task WithProduct( IClient client, Func<ProductDraft, ProductDraft> draftAction, Func<Product, Task> func)
        {
            await WithProductType(client, async productType =>
            {
                var productDraftWithProductType = new ProductDraft
                {
                    ProductType = productType.ToKeyResourceIdentifier()
                };

                await WithAsync(client, productDraftWithProductType, draftAction, func, null, DeleteProduct);
            });
        }
        #endregion

        #region WithUpdateableProduct

        public static async Task WithUpdateableProduct(IClient client, Func<Product, Product> func)
        {
            await WithProductType(client, async productType =>
            {
                var productDraftWithProductType = new ProductDraft
                {
                    ProductType = productType.ToKeyResourceIdentifier()
                };
                await WithUpdateable(client, productDraftWithProductType,
                    DefaultProductDraft, func, null, DeleteProduct);
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
                var productDraftWithProductType = new ProductDraft
                {
                    ProductType = productType.ToKeyResourceIdentifier()
                };
                await WithUpdateableAsync(client, productDraftWithProductType,
                    DefaultProductDraft, func, null, DeleteProduct);
            });
        }
        public static async Task WithUpdateableProduct(IClient client, Func<ProductDraft, ProductDraft> draftAction, Func<Product, Task<Product>> func)
        {
            await WithProductType(client, async productType =>
            {
                var productDraftWithProductType = new ProductDraft
                {
                    ProductType = productType.ToKeyResourceIdentifier()
                };
                await WithUpdateableAsync(client, productDraftWithProductType, draftAction, func, null, DeleteProduct);
            });
            
        }

        #endregion


        public static async Task DeleteProduct(IClient client, Product product)
        {
            try
            {
                var toBeDeleted = product;
                if (product.MasterData.Published)
                {
                    toBeDeleted = await Unpublish(client, product);
                }

                await DeleteResource(client, toBeDeleted);
            }
            catch (NotFoundException)
            {
            }
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
