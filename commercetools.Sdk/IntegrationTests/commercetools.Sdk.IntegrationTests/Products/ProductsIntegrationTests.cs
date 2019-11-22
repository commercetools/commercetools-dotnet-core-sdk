using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.Products.UpdateActions;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;
using static commercetools.Sdk.IntegrationTests.ProductDiscounts.ProductDiscountsFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using static commercetools.Sdk.IntegrationTests.Categories.CategoriesFixture;
using static commercetools.Sdk.IntegrationTests.TaxCategories.TaxCategoriesFixture;
using static commercetools.Sdk.IntegrationTests.States.StatesFixture;

namespace commercetools.Sdk.IntegrationTests.Products
{
    [Collection("Integration Tests")]
    public class ProductsIntegrationTests
    {
        private readonly IClient client;

        public ProductsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateProduct()
        {
            var key = $"CreateProduct-{TestingUtility.RandomString()}";
            await WithProduct(
                client, productDraft => DefaultProductDraftWithKey(productDraft, key),
                product => { Assert.Equal(key, product.Key); });
        }

        [Fact]
        public async Task GetProductById()
        {
            var key = $"GetProductById-{TestingUtility.RandomString()}";
            await WithProduct(
                client, productDraft => DefaultProductDraftWithKey(productDraft, key),
                async product =>
                {
                    var retrievedProduct = await client
                        .ExecuteAsync(product.ToIdResourceIdentifier().GetById());
                    Assert.Equal(key, retrievedProduct.Key);
                });
        }

        [Fact]
        public async Task GetProductByKey()
        {
            var key = $"GetProductByKey-{TestingUtility.RandomString()}";
            await WithProduct(
                client, productDraft => DefaultProductDraftWithKey(productDraft, key),
                async product =>
                {
                    var retrievedProduct = await client
                        .ExecuteAsync(product.ToKeyResourceIdentifier().GetByKey());
                    Assert.Equal(key, retrievedProduct.Key);
                });
        }

        [Fact]
        public async Task QueryProducts()
        {
            var key = $"QueryProducts-{TestingUtility.RandomString()}";
            await WithProduct(
                client, productDraft => DefaultProductDraftWithKey(productDraft, key),
                async product =>
                {
                    var queryCommand = new QueryCommand<Product>();
                    queryCommand.Where(p => p.Key == product.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteProductById()
        {
            var key = $"DeleteProductById-{TestingUtility.RandomString()}";
            await WithProduct(
                client, productDraft => DefaultProductDraftWithKey(productDraft, key),
                async product =>
                {
                    //unpublish it before deleting
                    var toBeDeleted = product;
                    if (product.MasterData.Published)
                    {
                        toBeDeleted = await Unpublish(client, product);
                    }

                    await client.ExecuteAsync(toBeDeleted.DeleteById());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Product>(toBeDeleted))
                    );
                });
        }

        [Fact]
        public async Task DeleteProductByKey()
        {
            var key = $"DeleteProductByKey-{TestingUtility.RandomString()}";
            await WithProduct(
                client, productDraft => DefaultProductDraftWithKey(productDraft, key),
                async product =>
                {
                    //unpublish it before deleting
                    var toBeDeleted = product;
                    if (product.MasterData.Published)
                    {
                        toBeDeleted = await Unpublish(client, product);
                    }

                    await client.ExecuteAsync(toBeDeleted.DeleteByKey());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Product>(toBeDeleted))
                    );
                });
        }
        
        [Fact]
        public async Task PublishProduct()
        {
            var key = $"Publish-{TestingUtility.RandomString()}";
            await WithUpdateableProduct(
                client, draft =>
                {
                    var productDraft = DefaultProductDraftWithKey(draft, key);
                    productDraft.Publish = false;
                    return productDraft;
                },
                async product =>
                {
                    Assert.False(product.MasterData.Published);
                    
                    var action = new PublishUpdateAction()
                    {
                        Scope = PublishScope.All
                    };

                    var publishedProduct = await client
                        .ExecuteAsync(product.UpdateByKey(actions => actions.AddUpdate(action)));
                    
                    Assert.True(publishedProduct.MasterData.Published);
                    return publishedProduct;
                });
        }
        
        [Fact]
        public async Task UnpublishProduct()
        {
            var key = $"Unpublish-{TestingUtility.RandomString()}";
            await WithUpdateableProduct(
                client, draft =>
                {
                    var productDraft = DefaultProductDraftWithKey(draft, key);
                    productDraft.Publish = true;
                    return productDraft;
                },
                async product =>
                {
                    Assert.True(product.MasterData.Published);
                    
                    var action = new UnpublishUpdateAction();

                    var unpublishedProduct = await client
                        .ExecuteAsync(product.UpdateByKey(actions => actions.AddUpdate(action)));
                    
                    Assert.False(unpublishedProduct.MasterData.Published);
                    return unpublishedProduct;
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateProductByKeyChangeName()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var name = TestingUtility.RandomString();
                var action = new ChangeNameUpdateAction
                {
                    Name = new LocalizedString
                    {
                        {"en", name}
                    }
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(name, updatedProduct.MasterData.Staged.Name["en"]);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductByIdChangeNameOnCurrent()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var name = TestingUtility.RandomString();
                var action = new ChangeNameUpdateAction
                {
                    Name = new LocalizedString
                    {
                        {"en", name}
                    },
                    Staged = false
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(name, updatedProduct.MasterData.Current.Name["en"]);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetKey()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var key = TestingUtility.RandomString();
                var action = new SetKeyUpdateAction
                {
                    Key = key
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(key, updatedProduct.Key);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetDescription()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var description = TestingUtility.RandomString();
                var action = new SetDescriptionUpdateAction
                {
                    Description = new LocalizedString
                    {
                        {"en", description}
                    }
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(description, updatedProduct.MasterData.Staged.Description["en"]);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductChangeSlug()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var slug = TestingUtility.RandomString();
                var action = new ChangeSlugUpdateAction
                {
                    Slug = new LocalizedString
                    {
                        {"en", slug}
                    }
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(slug, updatedProduct.MasterData.Staged.Slug["en"]);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductAddProductVariant()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.Empty(product.MasterData.Staged.Variants);

                var randInt = TestingUtility.RandomInt();
                var key = $"variantKey-{randInt}";
                var sku = $"variantSku-{randInt}";

                var action = new AddProductVariantUpdateAction
                {
                    Key = key,
                    Sku = sku
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedProduct.MasterData.Staged.Variants);
                var addedVariant = updatedProduct.MasterData.Staged.Variants[0];
                Assert.Equal(key, addedVariant.Key);
                Assert.Equal(sku, addedVariant.Sku);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductRemoveProductVariantBySku()
        {
            var variantsCount = 2;
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithMultipleVariants(productDraft, variantsCount),
                async product =>
                {
                    Assert.Equal(variantsCount, product.MasterData.Staged.Variants.Count);

                    var skuOfRemovedVariant = product.MasterData.Staged.Variants[0].Sku;

                    var action = new RemoveProductVariantUpdateAction(skuOfRemovedVariant);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(variantsCount - 1, updatedProduct.MasterData.Staged.Variants.Count);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductRemoveProductVariantByIdOnCurrent()
        {
            var variantsCount = 2;
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithMultipleVariants(productDraft, variantsCount),
                async product =>
                {
                    Assert.Equal(variantsCount, product.MasterData.Staged.Variants.Count);

                    var idOfRemovedVariant = product.MasterData.Staged.Variants[0].Id;

                    var action = new RemoveProductVariantUpdateAction(idOfRemovedVariant, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(variantsCount - 1, updatedProduct.MasterData.Current.Variants.Count);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductChangeMasterVariantBySku()
        {
            var variantsCount = 2;
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithMultipleVariants(productDraft, variantsCount),
                async product =>
                {
                    Assert.Equal(variantsCount, product.MasterData.Staged.Variants.Count);

                    var skuOfVariant = product.MasterData.Staged.Variants[0].Sku;

                    var action = new ChangeMasterVariantUpdateAction(skuOfVariant);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(skuOfVariant, updatedProduct.MasterData.Staged.MasterVariant.Sku);
                    return updatedProduct;
                });
        }


        [Fact]
        public async Task UpdateProductAddPriceBySku()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var skuOfMasterVariant = product.MasterData.Staged.MasterVariant.Sku;
                var priceCentAmount = TestingUtility.RandomInt(1000, 5000);
                var pricesCount = product.MasterData.Staged.MasterVariant.Prices.Count;

                var newProductPrice = TestingUtility.GetPriceDraft(priceCentAmount, DateTime.Now.AddMonths(6),
                    DateTime.Now.AddMonths(7));

                var action = new AddPriceUpdateAction(skuOfMasterVariant, newProductPrice);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(pricesCount + 1, updatedProduct.MasterData.Staged.MasterVariant.Prices.Count);
                Assert.Contains(updatedProduct.MasterData.Staged.MasterVariant.Prices,
                    price => price.Value.CentAmount == priceCentAmount);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductAddPriceByVariantId()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var masterVariantId = product.MasterData.Staged.MasterVariant.Id;
                var priceCentAmount = TestingUtility.RandomInt(1000, 5000);
                var pricesCount = product.MasterData.Staged.MasterVariant.Prices.Count;

                var newProductPrice = TestingUtility.GetPriceDraft(priceCentAmount, DateTime.Now.AddMonths(6),
                    DateTime.Now.AddMonths(7));

                var action = new AddPriceUpdateAction(masterVariantId, newProductPrice, staged: false);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(pricesCount + 1, updatedProduct.MasterData.Current.MasterVariant.Prices.Count);
                Assert.Contains(updatedProduct.MasterData.Current.MasterVariant.Prices,
                    price => price.Value.CentAmount == priceCentAmount);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetPricesBySku()
        {
            var pricesCount = 3;
            await WithUpdateableProduct(client, async product =>
            {
                Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

                var skuOfMasterVariant = product.MasterData.Staged.MasterVariant.Sku;
                var prices = TestingUtility.GetRandomListOfPriceDraft(pricesCount);
                var price1 = prices[0];

                var action = new SetPricesUpdateAction(skuOfMasterVariant, prices);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(pricesCount, updatedProduct.MasterData.Staged.MasterVariant.Prices.Count);
                Assert.Contains(updatedProduct.MasterData.Staged.MasterVariant.Prices,
                    price => price.Value.CentAmount == price1.Value.CentAmount);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetPricesByVariantId()
        {
            var pricesCount = 3;
            await WithUpdateableProduct(client, async product =>
            {
                Assert.NotEmpty(product.MasterData.Current.MasterVariant.Prices);

                var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                var prices = TestingUtility.GetRandomListOfPriceDraft(pricesCount);
                var price1 = prices[0];

                var action = new SetPricesUpdateAction(masterVariantId, prices, staged: false);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(pricesCount, updatedProduct.MasterData.Current.MasterVariant.Prices.Count);
                Assert.Contains(updatedProduct.MasterData.Current.MasterVariant.Prices,
                    price => price.Value.CentAmount == price1.Value.CentAmount);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductChangePrice()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

                var oldPrice = product.MasterData.Staged.MasterVariant.Prices[0];
                var newPriceCentAmount = TestingUtility.RandomInt(1000, 10000);
                var newProductPrice = TestingUtility.GetPriceDraft(newPriceCentAmount);

                var action = new ChangePriceUpdateAction
                {
                    Price = newProductPrice,
                    PriceId = oldPrice.Id
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.NotEmpty(updatedProduct.MasterData.Staged.MasterVariant.Prices);
                Assert.Equal(newProductPrice.Value, updatedProduct.MasterData.Staged.MasterVariant.Prices[0].Value);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductRemovePrice()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

                var removePrice = product.MasterData.Staged.MasterVariant.Prices[0];
                var pricesCount = product.MasterData.Staged.MasterVariant.Prices.Count;

                var action = new RemovePriceUpdateAction
                {
                    PriceId = removePrice.Id,
                    Staged = true
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(pricesCount - 1, updatedProduct.MasterData.Staged.MasterVariant.Prices.Count);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetPriceCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableProduct(client, async product =>
                {
                    Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

                    var price = product.MasterData.Staged.MasterVariant.Prices[0];

                    var action = new SetPriceCustomTypeUpdateAction
                    {
                        PriceId = price.Id,
                        Type = type.ToKeyResourceIdentifier(),
                        Fields = fields
                    };

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    var updatedPrice = updatedProduct.MasterData.Staged.MasterVariant.Prices[0];
                    Assert.Equal(type.Id, updatedPrice.Custom.Type.Id);
                    return updatedProduct;
                });
            });
        }

        [Fact]
        public async Task UpdateProductSetPriceCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableProduct(client,
                    draft =>
                    {
                        var productDraft = DefaultProductDraft(draft);
                        productDraft.MasterVariant.Prices[0].Custom = new CustomFieldsDraft
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };
                        return productDraft;
                    },
                    async product =>
                    {
                        Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

                        var price = product.MasterData.Staged.MasterVariant.Prices[0];
                        Assert.NotNull(price.Custom);
                        Assert.Equal(type.Id, price.Custom.Type.Id);

                        var action = new SetPriceCustomFieldUpdateAction
                        {
                            PriceId = price.Id,
                            Name = "string-field",
                            Value = newValue
                        };

                        var updatedProduct = await client
                            .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                        var updatedPrice = updatedProduct.MasterData.Staged.MasterVariant.Prices[0];
                        Assert.NotNull(updatedPrice.Custom);
                        Assert.Equal(type.Id, updatedPrice.Custom.Type.Id);
                        Assert.Equal(newValue, updatedPrice.Custom.Fields["string-field"]);
                        return updatedProduct;
                    });
            });
        }

        [Fact]
        public async Task UpdateProductSetDiscountedPrice()
        {
            await WithProductDiscount(client,
                draft =>
                {
                    var productDiscountDraft = DefaultProductDiscountDraft(draft);
                    productDiscountDraft.IsActive = true;
                    productDiscountDraft.Value = new ExternalProductDiscountValue();
                    return productDiscountDraft;
                },
                async productDiscount =>
                {
                    await WithUpdateableProduct(client, async product =>
                    {
                        Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

                        var price = product.MasterData.Staged.MasterVariant.Prices[0];
                        var discountedPrice = new DiscountedPrice
                        {
                            Value = Money.FromDecimal("EUR", 100),
                            Discount = new Reference<ProductDiscount> {Id = productDiscount.Id}
                        };
                        var action = new SetDiscountedPriceUpdateAction
                        {
                            PriceId = price.Id,
                            Discounted = discountedPrice,
                            Staged = true
                        };

                        var updatedProduct = await client
                            .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.NotNull(updatedProduct.MasterData.Staged.MasterVariant.Prices[0].Discounted);
                        Assert.Equal(discountedPrice.Value,
                            updatedProduct.MasterData.Staged.MasterVariant.Prices[0].Discounted.Value);
                        Assert.Equal(discountedPrice.Discount.Id,
                            updatedProduct.MasterData.Staged.MasterVariant.Prices[0].Discounted.Discount.Id);
                        return updatedProduct;
                    });
                });
        }

        [Fact]
        public async Task UpdateProductSetAttribute()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var textAttributeName = "text-attribute-name";
                var sku = product.MasterData.Current.MasterVariant.Sku;
                var newTextAttributeValue = TestingUtility.RandomString(10);

                var action = new SetAttributeUpdateAction(sku, textAttributeName, newTextAttributeValue);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                var productAttributeValue =
                    updatedProduct.MasterData.Staged.MasterVariant.GetTextAttributeValue(textAttributeName);

                Assert.NotNull(productAttributeValue);
                Assert.Equal(newTextAttributeValue, productAttributeValue);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetAttributeInAllVariants()
        {
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithMultipleVariants(productDraft),
                async product =>
                {
                    Assert.NotEmpty(product.MasterData.Staged.Variants);
                    var textAttributeName = "text-attribute-name";
                    var newTextAttributeValue = TestingUtility.RandomString(10);

                    var action = new SetAttributeInAllVariantsUpdateAction
                    {
                        Name = textAttributeName,
                        Value = newTextAttributeValue,
                        Staged = true
                    };

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    var productAttributeValue =
                        updatedProduct.MasterData.Staged.MasterVariant.GetTextAttributeValue(textAttributeName);
                    var productAttributeValueVariant =
                        updatedProduct.MasterData.Staged.Variants[0].GetTextAttributeValue(textAttributeName);

                    Assert.NotNull(productAttributeValue);
                    Assert.Equal(newTextAttributeValue, productAttributeValue);
                    Assert.Equal(newTextAttributeValue, productAttributeValueVariant);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductAddToCategory()
        {
            await WithCategory(client, async category =>
            {
                await WithUpdateableProduct(client, async product =>
                {
                    Assert.Empty(product.MasterData.Staged.Categories);
                    var action = new AddToCategoryUpdateAction
                    {
                        OrderHint = TestingUtility.RandomSortOrder(),
                        Category = category.ToKeyResourceIdentifier()
                    };

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedProduct.MasterData.Staged.Categories);
                    Assert.Equal(category.Id, updatedProduct.MasterData.Staged.Categories[0].Id);
                    return updatedProduct;
                });
            });
        }

        [Fact]
        public async Task UpdateProductSetCategoryOrderHintInCurrent()
        {
            await WithCategory(client, async category =>
            {
                await WithUpdateableProduct(client,
                    productDraft => DefaultProductDraftWithCategory(productDraft, category),
                    async product =>
                    {
                        var newOrderHint = TestingUtility.RandomSortOrder();
                        Assert.Single(product.MasterData.Current.Categories);
                        Assert.Equal(category.Id, product.MasterData.Current.Categories[0].Id);
                        var action = new SetCategoryOrderHintUpdateAction
                        {
                            OrderHint = newOrderHint,
                            CategoryId = category.Id,
                            Staged = false
                        };

                        var updatedProduct = await client
                            .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action))
                                .Expand(p => p.MasterData.Current.Categories.ExpandAll()));

                        Assert.Single(updatedProduct.MasterData.Current.Categories);
                        Assert.Equal(newOrderHint, updatedProduct.MasterData.Current.CategoryOrderHints[category.Id]);
                        return updatedProduct;
                    });
            });
        }

        [Fact]
        public async Task UpdateProductRemoveFromCategoryInCurrent()
        {
            await WithCategory(client, async category =>
            {
                await WithUpdateableProduct(client,
                    productDraft => DefaultProductDraftWithCategory(productDraft, category),
                    async product =>
                    {
                        Assert.Single(product.MasterData.Current.Categories);
                        Assert.Equal(category.Id, product.MasterData.Current.Categories[0].Id);

                        var action = new RemoveFromCategoryUpdateAction
                        {
                            Category = category.ToKeyResourceIdentifier(),
                            Staged = false
                        };

                        var updatedProduct = await client
                            .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.Empty(updatedProduct.MasterData.Current.Categories);
                        return updatedProduct;
                    });
            });
        }

        [Fact]
        public async Task UpdateProductSetTaxCategory()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithUpdateableProduct(client, async product =>
                {
                    Assert.Null(product.TaxCategory);
                    var action = new SetTaxCategoryUpdateAction
                    {
                        TaxCategory = taxCategory.ToKeyResourceIdentifier()
                    };

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.NotNull(updatedProduct.TaxCategory);
                    Assert.Equal(taxCategory.Id, updatedProduct.TaxCategory.Id);
                    return updatedProduct;
                });
            });
        }

        [Fact]
        public async Task UpdateProductSetSku()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var sku = TestingUtility.RandomString();
                var action = new SetSkuUpdateAction
                {
                    Sku = sku,
                    VariantId = product.MasterData.Staged.MasterVariant.Id
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(sku, updatedProduct.MasterData.Staged.MasterVariant.Sku);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetProductVariantKeyBySku()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var key = TestingUtility.RandomString();
                var sku = product.MasterData.Staged.MasterVariant.Sku;
                var action = new SetProductVariantKeyUpdateAction(sku, key);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(key, updatedProduct.MasterData.Staged.MasterVariant.Key);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetProductVariantKeyById()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var key = TestingUtility.RandomString();
                var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                var action = new SetProductVariantKeyUpdateAction(masterVariantId, key, staged: false);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(key, updatedProduct.MasterData.Current.MasterVariant.Key);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductAddExternalImageBySku()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.Empty(product.MasterData.Staged.MasterVariant.Images);
                var sku = product.MasterData.Staged.MasterVariant.Sku;
                var image = new Image
                {
                    Label = $"Test-External-Image-{TestingUtility.RandomInt()}",
                    Url = TestingUtility.ExternalImageUrl,
                    Dimensions = new Dimensions {W = 50, H = 50}
                };

                var action = new AddExternalImageUpdateAction(sku, image);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedProduct.MasterData.Staged.MasterVariant.Images);
                Assert.Equal(image.Url, updatedProduct.MasterData.Staged.MasterVariant.Images[0].Url);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductAddExternalImageByVariantId()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.Empty(product.MasterData.Current.MasterVariant.Images);
                var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                var image = new Image
                {
                    Label = $"Test-External-Image-{TestingUtility.RandomInt()}",
                    Url = TestingUtility.ExternalImageUrl,
                    Dimensions = new Dimensions {W = 50, H = 50}
                };

                var action = new AddExternalImageUpdateAction(masterVariantId, image, false);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedProduct.MasterData.Current.MasterVariant.Images);
                Assert.Equal(image.Url, updatedProduct.MasterData.Current.MasterVariant.Images[0].Url);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductMoveImageToPositionBySku()
        {
            var images = TestingUtility.GetListOfImages(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithImages(productDraft, images),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Images.Count);
                    //move second image to first position
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var image = product.MasterData.Staged.MasterVariant.Images[1];

                    var action = new MoveImageToPositionUpdateAction(sku, image.Url, 0, true);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Images.Count);
                    Assert.Equal(image.Label, updatedProduct.MasterData.Staged.MasterVariant.Images[0].Label);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductMoveImageToPositionByVariantId()
        {
            var images = TestingUtility.GetListOfImages(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithImages(productDraft, images),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Images.Count);
                    //move second image to first position
                    var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                    var image = product.MasterData.Current.MasterVariant.Images[1];

                    var action = new MoveImageToPositionUpdateAction(masterVariantId, image.Url, 0, false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(3, updatedProduct.MasterData.Current.MasterVariant.Images.Count);
                    Assert.Equal(image.Label, updatedProduct.MasterData.Current.MasterVariant.Images[0].Label);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductRemoveImageBySku()
        {
            var images = TestingUtility.GetListOfImages(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithImages(productDraft, images),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Images.Count);
                    //remove second image
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var image = product.MasterData.Staged.MasterVariant.Images[1];

                    var action = new RemoveImageUpdateAction(sku, image.Url);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(2, updatedProduct.MasterData.Staged.MasterVariant.Images.Count);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductRemoveImageByVariantId()
        {
            var images = TestingUtility.GetListOfImages(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithImages(productDraft, images),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Images.Count);
                    //remove second image
                    var masterVariantId = product.MasterData.Current.MasterVariant.Sku;
                    var image = product.MasterData.Current.MasterVariant.Images[1];

                    var action = new RemoveImageUpdateAction(masterVariantId, image.Url, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(2, updatedProduct.MasterData.Current.MasterVariant.Images.Count);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetImageLabelBySku()
        {
            var images = TestingUtility.GetListOfImages(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithImages(productDraft, images),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Images.Count);

                    //update the first image label
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var image = product.MasterData.Staged.MasterVariant.Images[0];
                    var newLabel = TestingUtility.RandomString();

                    var action = new SetImageLabelUpdateAction(sku, image.Url, newLabel);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Images.Count);
                    Assert.Equal(newLabel, updatedProduct.MasterData.Staged.MasterVariant.Images[0].Label);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetImageLabelByVariantId()
        {
            var images = TestingUtility.GetListOfImages(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithImages(productDraft, images),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Images.Count);

                    //update the first image label
                    var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                    var image = product.MasterData.Current.MasterVariant.Images[0];
                    var newLabel = TestingUtility.RandomString();

                    var action = new SetImageLabelUpdateAction(masterVariantId, image.Url, newLabel, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(3, updatedProduct.MasterData.Current.MasterVariant.Images.Count);
                    Assert.Equal(newLabel, updatedProduct.MasterData.Current.MasterVariant.Images[0].Label);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductAddAssetBySku()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.Empty(product.MasterData.Staged.MasterVariant.Assets);
                var sku = product.MasterData.Staged.MasterVariant.Sku;
                var asset = TestingUtility.GetAssetDraft();

                var action = new AddAssetUpdateAction(sku, asset, position: 0);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedProduct.MasterData.Staged.MasterVariant.Assets);
                Assert.Equal(asset.Key, updatedProduct.MasterData.Staged.MasterVariant.Assets[0].Key);

                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductAddAssetByVariantId()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.Empty(product.MasterData.Current.MasterVariant.Assets);
                var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                var asset = TestingUtility.GetAssetDraft();

                var action = new AddAssetUpdateAction(masterVariantId, asset, position: 0, staged: false);

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedProduct.MasterData.Current.MasterVariant.Assets);
                Assert.Equal(asset.Key, updatedProduct.MasterData.Current.MasterVariant.Assets[0].Key);

                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductRemoveAssetBySkuAndAssetKey()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

                    //remove second asset By Sku and assetKey
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var asset = product.MasterData.Staged.MasterVariant.Assets[1];

                    var action =
                        new RemoveAssetUpdateAction(sku: sku, assetId: null, assetKey: asset.Key, staged: true);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(2, updatedProduct.MasterData.Staged.MasterVariant.Assets.Count);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductRemoveAssetByVariantIdAndAssetId()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Assets.Count);

                    //remove second asset
                    var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                    var asset = product.MasterData.Current.MasterVariant.Assets[1];

                    var action = new RemoveAssetUpdateAction(variantId: masterVariantId,
                        assetId: asset.Id, assetKey: null, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(2, updatedProduct.MasterData.Current.MasterVariant.Assets.Count);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetAssetKeyBySku()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

                    //set second asset Key
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var asset = product.MasterData.Staged.MasterVariant.Assets[1];
                    var key = TestingUtility.RandomString();

                    var action =
                        new SetAssetKeyUpdateAction(sku: sku, assetId: asset.Id, assetKey: key, staged: true);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Assets.Count);
                    Assert.Equal(key, updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Key);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetAssetKeyByVariantId()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Assets.Count);

                    //set second asset Key
                    var masterVariantId = product.MasterData.Staged.MasterVariant.Id;
                    var asset = product.MasterData.Staged.MasterVariant.Assets[1];
                    var key = TestingUtility.RandomString();

                    var action =
                        new SetAssetKeyUpdateAction(variantId: masterVariantId,
                            assetId: asset.Id, assetKey: key, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(3, updatedProduct.MasterData.Current.MasterVariant.Assets.Count);
                    Assert.Equal(key, updatedProduct.MasterData.Current.MasterVariant.Assets[1].Key);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductChangeAssetOrderBySku()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

                    //reverse the assets orders
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var productAssets = product.MasterData.Staged.MasterVariant.Assets;
                    productAssets.Reverse();
                    var reversedAssetsOrder = productAssets.Select(asset => asset.Id).ToList();

                    var action =
                        new ChangeAssetOrderUpdateAction(sku, reversedAssetsOrder);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    var newAssetsOrder = updatedProduct.MasterData.Staged.MasterVariant.Assets.Select(asset => asset.Id)
                        .ToList();

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Assets.Count);
                    Assert.Equal(reversedAssetsOrder, newAssetsOrder);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductChangeAssetOrderByVariantId()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Assets.Count);

                    //reverse the assets orders
                    var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                    var productAssets = product.MasterData.Current.MasterVariant.Assets;
                    productAssets.Reverse();
                    var reversedAssetsOrder = productAssets.Select(asset => asset.Id).ToList();

                    var action =
                        new ChangeAssetOrderUpdateAction(masterVariantId,
                            reversedAssetsOrder, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    var newAssetsOrder = updatedProduct.MasterData.Current.MasterVariant.Assets
                        .Select(asset => asset.Id)
                        .ToList();

                    Assert.Equal(3, updatedProduct.MasterData.Current.MasterVariant.Assets.Count);
                    Assert.Equal(reversedAssetsOrder, newAssetsOrder);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductChangeAssetNameBySku()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

                    //change name of the second asset By Sku and Asset Key
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var asset = product.MasterData.Staged.MasterVariant.Assets[1];
                    var name = new LocalizedString() {{"en", TestingUtility.RandomString()}};

                    var action =
                        new ChangeAssetNameUpdateAction(sku: sku, assetKey: asset.Key, name: name);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Assets.Count);
                    Assert.Equal(name["en"], updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Name["en"]);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductChangeAssetNameByVariantId()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Assets.Count);

                    //change name of the second asset By Sku and Asset Key
                    var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                    var asset = product.MasterData.Current.MasterVariant.Assets[1];
                    var name = new LocalizedString() {{"en", TestingUtility.RandomString()}};

                    var action =
                        new ChangeAssetNameUpdateAction(variantId: masterVariantId,
                            assetKey: null, assetId: asset.Id, name: name, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(3, updatedProduct.MasterData.Current.MasterVariant.Assets.Count);
                    Assert.Equal(name["en"], updatedProduct.MasterData.Current.MasterVariant.Assets[1].Name["en"]);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetAssetDescriptionBySku()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

                    //change Description of the second asset By Sku and Asset Key
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var asset = product.MasterData.Staged.MasterVariant.Assets[1];
                    var description = new LocalizedString() {{"en", TestingUtility.RandomString()}};

                    var action =
                        new SetAssetDescriptionUpdateAction(sku: sku, assetKey: asset.Key, description: description);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Assets.Count);
                    Assert.Equal(description["en"],
                        updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Description["en"]);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetAssetDescriptionByVariantId()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Assets.Count);

                    //change name of the second asset By Sku and Asset Key
                    var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                    var asset = product.MasterData.Current.MasterVariant.Assets[1];
                    var description = new LocalizedString() {{"en", TestingUtility.RandomString()}};

                    var action =
                        new SetAssetDescriptionUpdateAction(variantId: masterVariantId,
                            assetKey: null, assetId: asset.Id, description: description, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(3, updatedProduct.MasterData.Current.MasterVariant.Assets.Count);
                    Assert.Equal(description["en"],
                        updatedProduct.MasterData.Current.MasterVariant.Assets[1].Description["en"]);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetAssetTagsBySku()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

                    //set Tags for the second asset By Sku and Asset Key
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var asset = product.MasterData.Staged.MasterVariant.Assets[1];
                    var newTags = new List<string> {"Tag1"};

                    var action =
                        new SetAssetTagsUpdateAction(sku: sku, assetKey: asset.Key, tags: newTags);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Assets.Count);
                    Assert.Equal(newTags, updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Tags);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetAssetTagsByVariantId()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Assets.Count);

                    //set Tags for the second asset By Sku and Asset Key
                    var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                    var asset = product.MasterData.Current.MasterVariant.Assets[1];
                    var newTags = new List<string> {"Tag1"};

                    var action =
                        new SetAssetTagsUpdateAction(variantId: masterVariantId,
                            assetKey: null, assetId: asset.Id, tags: newTags, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(3, updatedProduct.MasterData.Current.MasterVariant.Assets.Count);
                    Assert.Equal(newTags, updatedProduct.MasterData.Current.MasterVariant.Assets[1].Tags);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetAssetSourcesBySku()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

                    //set Tags for the second asset By Sku and Asset Key
                    var sku = product.MasterData.Staged.MasterVariant.Sku;
                    var asset = product.MasterData.Staged.MasterVariant.Assets[1];
                    var assetSources = TestingUtility.GetListOfAssetSource();

                    var action =
                        new SetAssetSourcesUpdateAction(sku: sku, assetKey: asset.Key, sources: assetSources);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.True(updatedProduct.MasterData.HasStagedChanges);
                    Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Assets.Count);
                    Assert.Equal(2, updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Sources.Count);
                    Assert.Equal(assetSources[0].Key,
                        updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Sources[0].Key);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetAssetSourcesByVariantId()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);
            await WithUpdateableProduct(client,
                productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                async product =>
                {
                    Assert.Equal(3, product.MasterData.Current.MasterVariant.Assets.Count);

                    //set Tags for the second asset By Sku and Asset Key
                    var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                    var asset = product.MasterData.Current.MasterVariant.Assets[1];
                    var assetSources = TestingUtility.GetListOfAssetSource();

                    var action =
                        new SetAssetSourcesUpdateAction(variantId: masterVariantId,
                            assetKey: asset.Key, sources: assetSources, staged: false);

                    var updatedProduct = await client
                        .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(3, updatedProduct.MasterData.Current.MasterVariant.Assets.Count);
                    Assert.Equal(2, updatedProduct.MasterData.Current.MasterVariant.Assets[1].Sources.Count);
                    Assert.Equal(assetSources[0].Key,
                        updatedProduct.MasterData.Current.MasterVariant.Assets[1].Sources[0].Key);
                    return updatedProduct;
                });
        }

        [Fact]
        public async Task UpdateProductSetAssetCustomTypeBySku()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);

            await WithType(client, async type =>
            {
                await WithUpdateableProduct(client,
                    productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                    async product =>
                    {
                        Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);
                        Assert.Null(product.MasterData.Staged.MasterVariant.Assets[1].Custom);

                        //set custom type of the second asset By Sku and Asset Key
                        var sku = product.MasterData.Staged.MasterVariant.Sku;
                        var asset = product.MasterData.Staged.MasterVariant.Assets[1];
                        var fields = CreateNewFields();
                        var typeResourceIdentifier = type.ToKeyResourceIdentifier();

                        var action =
                            new SetAssetCustomTypeUpdateAction(sku: sku, assetKey: asset.Key,
                                type: typeResourceIdentifier,
                                fields: fields);

                        var updatedProduct = await client
                            .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.True(updatedProduct.MasterData.HasStagedChanges);
                        Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Assets.Count);
                        Assert.NotNull(updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Type);
                        Assert.Equal(type.Id, updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Type.Id);
                        return updatedProduct;
                    });
            });
        }

        [Fact]
        public async Task UpdateProductSetAssetCustomTypeByVariantId()
        {
            var assets = TestingUtility.GetListOfAssetsDrafts(3);

            await WithType(client, async type =>
            {
                await WithUpdateableProduct(client,
                    productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                    async product =>
                    {
                        Assert.Equal(3, product.MasterData.Current.MasterVariant.Assets.Count);
                        Assert.Null(product.MasterData.Current.MasterVariant.Assets[1].Custom);

                        //set custom type of the second asset By Sku and Asset Key
                        var masterVariantId = product.MasterData.Current.MasterVariant.Id;
                        var asset = product.MasterData.Current.MasterVariant.Assets[1];
                        var fields = CreateNewFields();
                        var typeResourceIdentifier = type.ToKeyResourceIdentifier();

                        var action =
                            new SetAssetCustomTypeUpdateAction(variantId: masterVariantId,
                                assetKey: null, assetId: asset.Id, type: typeResourceIdentifier,
                                fields: fields, staged: false);

                        var updatedProduct = await client
                            .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.Equal(3, updatedProduct.MasterData.Current.MasterVariant.Assets.Count);
                        Assert.NotNull(updatedProduct.MasterData.Current.MasterVariant.Assets[1].Custom.Type);
                        Assert.Equal(type.Id, updatedProduct.MasterData.Current.MasterVariant.Assets[1].Custom.Type.Id);
                        return updatedProduct;
                    });
            });
        }

        [Fact]
        public async Task UpdateProductSetAssetCustomFieldBySku()
        {
            await WithType(client, async type =>
            {
                var fields = CreateNewFields();
                var assets = TestingUtility.GetListOfAssetsDrafts(3,
                    type.ToKeyResourceIdentifier(), fields);
                var stringFieldValue = TestingUtility.RandomString(10);

                await WithUpdateableProduct(client,
                    productDraft => DefaultProductDraftWithAssets(productDraft, assets),
                    async product =>
                    {
                        Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);
                        Assert.NotNull(product.MasterData.Staged.MasterVariant.Assets[1].Custom);

                        //set the custom field for asset 2
                        var sku = product.MasterData.Staged.MasterVariant.Sku;
                        var asset = product.MasterData.Staged.MasterVariant.Assets[1];

                        var action =
                            new SetAssetCustomFieldUpdateAction(sku: sku, assetKey: asset.Key, name: "string-field",
                                value: stringFieldValue);

                        var updatedProduct = await client
                            .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.True(updatedProduct.MasterData.HasStagedChanges);
                        Assert.Equal(3, updatedProduct.MasterData.Staged.MasterVariant.Assets.Count);
                        Assert.NotNull(updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Type);
                        Assert.Equal(type.Id, updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Type.Id);
                        Assert.Equal(stringFieldValue,
                            updatedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Fields["string-field"]);
                        return updatedProduct;
                    });
            });
        }

        [Fact]
        public async Task UpdateProductSetSearchKeywords()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.Empty(product.MasterData.Staged.SearchKeywords);

                var searchKeywords = TestingUtility.GetSearchKeywords();

                var action = new SetSearchKeywordsUpdateAction()
                {
                    SearchKeywords = searchKeywords,
                    Staged = true
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.True(updatedProduct.MasterData.HasStagedChanges);
                Assert.NotEmpty(updatedProduct.MasterData.Staged.SearchKeywords);
                Assert.NotEmpty(updatedProduct.MasterData.Staged.SearchKeywords["en"]);
                Assert.Equal(searchKeywords["en"][0].Text,
                    updatedProduct.MasterData.Staged.SearchKeywords["en"][0].Text);
                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetMetaTitle()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.Null(product.MasterData.Staged.MetaTitle);

                var metaTitle = new LocalizedString() {{"en", TestingUtility.RandomString()}};

                var action = new SetMetaTitleUpdateAction()
                {
                    MetaTitle = metaTitle,
                    Staged = true
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.True(updatedProduct.MasterData.HasStagedChanges);
                Assert.NotNull(updatedProduct.MasterData.Staged.MetaTitle);
                Assert.Equal(metaTitle["en"], updatedProduct.MasterData.Staged.MetaTitle["en"]);

                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetMetaDescription()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.Null(product.MasterData.Staged.MetaDescription);

                var metaDescription = new LocalizedString() {{"en", TestingUtility.RandomString()}};

                var action = new SetMetaDescriptionUpdateAction()
                {
                    MetaDescription = metaDescription,
                    Staged = true
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.True(updatedProduct.MasterData.HasStagedChanges);
                Assert.NotNull(updatedProduct.MasterData.Staged.MetaDescription);
                Assert.Equal(metaDescription["en"], updatedProduct.MasterData.Staged.MetaDescription["en"]);

                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductSetMetaKeywords()
        {
            await WithUpdateableProduct(client, async product =>
            {
                Assert.Null(product.MasterData.Staged.MetaKeywords);

                var metaKeywords = new LocalizedString() {{"en", TestingUtility.RandomString()}};

                var action = new SetMetaKeywordsUpdateAction()
                {
                    MetaKeywords = metaKeywords,
                    Staged = true
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                Assert.True(updatedProduct.MasterData.HasStagedChanges);
                Assert.NotNull(updatedProduct.MasterData.Staged.MetaKeywords);
                Assert.Equal(metaKeywords["en"], updatedProduct.MasterData.Staged.MetaKeywords["en"]);

                return updatedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductRevertStagedChanges()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var oldDescription = product.MasterData.Staged.Description;
                var newDescription = new LocalizedString() {{"en", TestingUtility.RandomString(20)}};

                var action = new SetDescriptionUpdateAction
                {
                    Description = newDescription,
                    Staged = true
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                // make sure that current description like the old description and only the staged with the new description
                Assert.Equal(newDescription["en"], updatedProduct.MasterData.Staged.Description["en"]);
                Assert.Equal(oldDescription["en"], updatedProduct.MasterData.Current.Description["en"]);

                //Create RevertAction and Update the Product to revert changes
                var revertStagedChangesUpdateAction = new RevertStagedChangesUpdateAction();

                var revertedProduct = await client
                    .ExecuteAsync(updatedProduct.UpdateById(actions =>
                        actions.AddUpdate(revertStagedChangesUpdateAction)));

                //make sure that current and staged has the old description
                Assert.Equal(oldDescription["en"], revertedProduct.MasterData.Current.Description["en"]);
                Assert.Equal(oldDescription["en"], revertedProduct.MasterData.Staged.Description["en"]);
                return revertedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductRevertStagedVariantChanges()
        {
            await WithUpdateableProduct(client, async product =>
            {
                var oldVariantSku = product.MasterData.Staged.MasterVariant.Sku;
                var newVariantSku = TestingUtility.RandomString(20);

                var masterVariantId = product.MasterData.Staged.MasterVariant.Id;
                var action = new SetSkuUpdateAction()
                {
                    Sku = newVariantSku,
                    VariantId = masterVariantId,
                    Staged = true
                };

                var updatedProduct = await client
                    .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                // make sure that current variant sku like the old sku and only the staged with the new sku
                Assert.Equal(newVariantSku, updatedProduct.MasterData.Staged.MasterVariant.Sku);
                Assert.Equal(oldVariantSku, updatedProduct.MasterData.Current.MasterVariant.Sku);

                //Create RevertAction and Update the Product to revert changes
                var revertStagedChangesUpdateAction = new RevertStagedVariantChangesUpdateAction
                {
                    VariantId = masterVariantId
                };

                var revertedProduct = await client
                    .ExecuteAsync(updatedProduct.UpdateById(actions =>
                        actions.AddUpdate(revertStagedChangesUpdateAction)));

                //make sure that current and staged has the old description
                Assert.Equal(oldVariantSku, revertedProduct.MasterData.Current.MasterVariant.Sku);
                Assert.Equal(oldVariantSku, revertedProduct.MasterData.Staged.MasterVariant.Sku);
                return revertedProduct;
            });
        }

        [Fact]
        public async Task UpdateProductTransitionToNewState()
        {
            await WithState(client, draft =>
                {
                    var stateDraft = DefaultStateDraftWithType(draft, StateType.ProductState);
                    stateDraft.Initial = true;
                    return stateDraft;
                },
                async initialProductState =>
                {
                    await WithUpdateableProduct(client, async product =>
                    {
                        Assert.Null(product.State);
                        var action = new TransitionStateUpdateAction()
                        {
                            State = initialProductState.ToKeyResourceIdentifier()
                        };

                        var updatedProduct = await client
                            .ExecuteAsync(product.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.NotNull(updatedProduct.State);
                        Assert.Equal(initialProductState.Id, updatedProduct.State.Id);
                        return updatedProduct;
                    });
                });
        }

        #endregion
    }
}