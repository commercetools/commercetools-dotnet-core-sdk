using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;
using static commercetools.Sdk.IntegrationTests.TaxCategories.TaxCategoriesFixture;
using static commercetools.Sdk.IntegrationTests.Customers.CustomersFixture;
using static commercetools.Sdk.IntegrationTests.OrdersImport.OrdersImportFixture;
using static commercetools.Sdk.IntegrationTests.ShippingMethods.ShippingMethodsFixture;
using static commercetools.Sdk.IntegrationTests.Stores.StoresFixture;

namespace commercetools.Sdk.IntegrationTests.OrdersImport
{
    [Collection("Integration Tests")]
    public class OrdersImportIntegrationTests
    {
        private readonly IClient client;

        public OrdersImportIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task TestImportMinimalOrder()
        {
            await WithProduct(client, async product =>
            {
                await WithImportOrder(
                    client,
                    draft => DefaultOrderImportDraftWithLineItemByProductId(draft, product.Id),
                    (order, orderImportDraft) =>
                    {
                        //Assert
                        Assert.Null(order.Cart);

                        Assert.Equal(orderImportDraft.TotalPrice, order.TotalPrice);
                        Assert.Equal(orderImportDraft.Country, order.Country);
                        Assert.Equal(orderImportDraft.ShippingAddress.ToString(), order.ShippingAddress.ToString());
                        Assert.Equal(orderImportDraft.BillingAddress.ToString(), order.BillingAddress.ToString());
                        Assert.Equal(orderImportDraft.OrderNumber, order.OrderNumber);

                        Assert.Equal(orderImportDraft.OrderState, order.OrderState);
                        Assert.Equal(orderImportDraft.PaymentState, order.PaymentState);
                        Assert.Equal(orderImportDraft.ShipmentState, order.ShipmentState);
                        Assert.Equal(orderImportDraft.InventoryMode, order.InventoryMode);
                        Assert.Equal(orderImportDraft.TaxRoundingMode, order.TaxRoundingMode);
                        Assert.Equal(orderImportDraft.TaxCalculationMode, order.TaxCalculationMode);


                        Assert.Equal(orderImportDraft.TaxedPrice.TotalNet, order.TaxedPrice.TotalNet);
                        Assert.Equal(orderImportDraft.TaxedPrice.TotalGross, order.TaxedPrice.TotalGross);

                        Assert.Single(order.LineItems);
                        var orderLineItem = order.LineItems[0];
                        var draftLineItem = orderImportDraft.LineItems[0];
                        Assert.True(draftLineItem.Name.DictionaryEqual(orderLineItem.Name));
                        Assert.Equal(draftLineItem.ProductId, orderLineItem.ProductId);
                        Assert.Equal(draftLineItem.Quantity, orderLineItem.Quantity);
                    }
                );
            });
        }
        
        [Fact]
        public async Task TestImportMinimalOrderBySku()
        {
            await WithProduct(client, async product =>
            {
                await WithImportOrder(
                    client,
                    draft => DefaultOrderImportDraftWithLineItemBySku(draft, product.MasterData.Staged.MasterVariant.Sku),
                    (order, orderImportDraft) =>
                    {
                        //Assert
                        Assert.Null(order.Cart);

                        Assert.Equal(orderImportDraft.TotalPrice, order.TotalPrice);
                        Assert.Equal(orderImportDraft.Country, order.Country);
                        Assert.Equal(orderImportDraft.ShippingAddress.ToString(), order.ShippingAddress.ToString());
                        Assert.Equal(orderImportDraft.BillingAddress.ToString(), order.BillingAddress.ToString());
                        Assert.Equal(orderImportDraft.OrderNumber, order.OrderNumber);

                        Assert.Equal(orderImportDraft.OrderState, order.OrderState);
                        Assert.Equal(orderImportDraft.PaymentState, order.PaymentState);
                        Assert.Equal(orderImportDraft.ShipmentState, order.ShipmentState);
                        Assert.Equal(orderImportDraft.InventoryMode, order.InventoryMode);
                        Assert.Equal(orderImportDraft.TaxRoundingMode, order.TaxRoundingMode);
                        Assert.Equal(orderImportDraft.TaxCalculationMode, order.TaxCalculationMode);


                        Assert.Equal(orderImportDraft.TaxedPrice.TotalNet, order.TaxedPrice.TotalNet);
                        Assert.Equal(orderImportDraft.TaxedPrice.TotalGross, order.TaxedPrice.TotalGross);

                        Assert.Single(order.LineItems);
                        var orderLineItem = order.LineItems[0];
                        var draftLineItem = orderImportDraft.LineItems[0];
                        Assert.True(draftLineItem.Name.DictionaryEqual(orderLineItem.Name));
                        Assert.Equal(draftLineItem.Variant.Sku, orderLineItem.Variant.Sku);
                        Assert.Equal(draftLineItem.Quantity, orderLineItem.Quantity);
                    }
                );
            });
        }

        [Fact]
        public async Task TestImportOrderWithCustomer()
        {
            await WithCustomer(client, async customer =>
            {
                await WithProduct(client, async product =>
                {
                    await WithImportOrder(
                        client,
                        draft =>
                        {
                            var orderImportDraft = DefaultOrderImportDraftWithLineItemByProductId(draft, product.Id);
                            orderImportDraft.CustomerId = customer.Id;
                            orderImportDraft.CustomerEmail = customer.Email;
                            return orderImportDraft;
                        },
                        (order, orderImportDraft) =>
                        {
                            //Assert
                            Assert.Null(order.Cart);
                            Assert.Equal(orderImportDraft.CustomerId, order.CustomerId);
                            Assert.Equal(orderImportDraft.CustomerEmail, order.CustomerEmail);
                        }
                    );
                });
            });
        }
        
        [Fact]
        public async Task TestImportOrderInStore()
        {
            await WithStore(client, async store =>
            {
                await WithProduct(client, async product =>
                {
                    await WithImportOrder(
                        client,
                        draft =>
                        {
                            var orderImportDraft = DefaultOrderImportDraftWithLineItemByProductId(draft, product.Id);
                            orderImportDraft.Store = store.ToKeyResourceIdentifier();
                            return orderImportDraft;
                        },
                        (order, orderImportDraft) =>
                        {
                            //Assert
                            Assert.Null(order.Cart);
                            Assert.NotNull(order.Store);
                            Assert.Equal(store.Key, order.Store.Key);
                        }
                    );
                });
            });
        }

        [Fact]
        public async Task TestImportOrderWithShippingInfo()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithShippingMethod(client, async shippingMethod =>
                {
                    await WithProduct(client, async product =>
                    {
                        await WithImportOrder(
                            client,
                            draft => DefaultOrderImportDraftWithLineItemWithShippingInfo(draft, product.Id, taxCategory,
                                shippingMethod),
                            (order, orderImportDraft) =>
                            {
                                //Assert
                                Assert.Null(order.Cart);
                                Assert.NotNull(order.ShippingInfo);
                                var shippingInfo = order.ShippingInfo;
                                Assert.Equal(orderImportDraft.ShippingInfo.Price, shippingInfo.Price);
                                Assert.Equal(orderImportDraft.ShippingInfo.ShippingMethodName,
                                    shippingInfo.ShippingMethodName);

                                var shippingRate = orderImportDraft.ShippingInfo.ShippingRate;
                                Assert.Equal(shippingMethod.Id, shippingInfo.ShippingMethod.Id);
                                Assert.Equal(taxCategory.Id, shippingInfo.TaxCategory.Id);
                                Assert.Equal(shippingRate.Price, order.ShippingInfo.ShippingRate.Price);
                                Assert.Equal(shippingRate.FreeAbove, order.ShippingInfo.ShippingRate.FreeAbove);
                            }
                        );
                    });
                });
            });
        }


        [Fact]
        public async Task TestImportOrderWithCustomLineItem()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithImportOrder(
                    client,
                    draft => DefaultOrderImportDraftWithCustomLineItem(draft, taxCategory),
                    (order, orderImportDraft) =>
                    {
                        //Assert
                        Assert.Null(order.Cart);
                        var orderCustomLineItem = order.CustomLineItems.FirstOrDefault();
                        Assert.NotNull(orderCustomLineItem);
                        var customLineItemDraft = orderImportDraft.CustomLineItems[0];
                        Assert.Equal(customLineItemDraft.Name, orderCustomLineItem.Name);
                        Assert.Equal(customLineItemDraft.Slug, orderCustomLineItem.Slug);
                        Assert.Equal(customLineItemDraft.Quantity, orderCustomLineItem.Quantity);
                        Assert.Equal(customLineItemDraft.Money, orderCustomLineItem.Money);
                        Assert.Equal(taxCategory.Id, orderCustomLineItem.TaxCategory.Id);
                    }
                );
            });
        }
    }
}
