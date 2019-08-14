using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.ShippingMethods;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;
using static commercetools.Sdk.IntegrationTests.TaxCategories.TaxCategoriesFixture;
using static commercetools.Sdk.IntegrationTests.Customers.CustomersFixture;
using static commercetools.Sdk.IntegrationTests.OrdersImport.OrdersImportFixture;
using static commercetools.Sdk.IntegrationTests.ShippingMethods.ShippingMethodsFixture;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

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
                //Arrange
                var orderImportDraft = DefaultOrderImportDraftWithLineItemByProductId(new OrderImportDraft(), product.Id);

                //Act
                var importOrderCommand = new ImportOrderCommand(orderImportDraft);
                var order = await client.ExecuteAsync(importOrderCommand);

                //Assert
                Assert.Null(order.Cart);

                Assert.Equal(orderImportDraft.TotalPrice, order.TotalPrice);
                Assert.Equal(orderImportDraft.Country, order.Country);
                Assert.True(orderImportDraft.ShippingAddress.EqualsIgnoreId(order.ShippingAddress));
                Assert.True(orderImportDraft.BillingAddress.EqualsIgnoreId(order.BillingAddress));
                Assert.Equal(orderImportDraft.OrderNumber, order.OrderNumber);

                Assert.Equal(orderImportDraft.OrderState, order.OrderState);
                Assert.Equal(orderImportDraft.PaymentState, order.PaymentState);
                Assert.Equal(orderImportDraft.ShipmentState, order.ShipmentState);
                Assert.Equal(orderImportDraft.InventoryMode, order.InventoryMode);
                Assert.Equal(orderImportDraft.TaxRoundingMode, order.TaxRoundingMode);
                Assert.Equal(orderImportDraft.TaxCalculationMode, order.TaxCalculationMode);


                Assert.True(AreEquals(orderImportDraft.TaxedPrice, order.TaxedPrice));

                Assert.Single(order.LineItems);
                var orderLineItem = order.LineItems[0];
                var draftLineItem = orderImportDraft.LineItems[0];
                Assert.Equal(draftLineItem.Name, orderLineItem.Name);
                Assert.Equal(draftLineItem.ProductId, orderLineItem.ProductId);
                Assert.Equal(draftLineItem.Quantity, orderLineItem.Quantity);

                //Delete it
                await DeleteResource(client, order);
            });
        }

        [Fact]
        public async Task TestImportOrderWithCustomer()
        {
            await WithCustomer(client, async customer =>
            {
                await WithProduct(client, async product =>
                {
                    //Arrange
                    var orderImportDraft = DefaultOrderImportDraftWithLineItemByProductId(new OrderImportDraft(), product.Id);
                    orderImportDraft.CustomerId = customer.Id;
                    orderImportDraft.CustomerEmail = customer.Email;

                    //Act
                    var importOrderCommand = new ImportOrderCommand(orderImportDraft);
                    var order = await client.ExecuteAsync(importOrderCommand);

                    //Assert
                    Assert.Null(order.Cart);
                    Assert.Equal(orderImportDraft.CustomerId, order.CustomerId);
                    Assert.Equal(orderImportDraft.CustomerEmail, order.CustomerEmail);

                    //Delete it
                    await DeleteResource(client, order);
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
                        //Arrange
                        var amountEuro10 = Money.FromDecimal("EUR", 10);
                        var shippingRate = TestingUtility.GetShippingRate();
                        var shippingInfoImportDraft = new ShippingInfoImportDraft
                        {
                            Price = amountEuro10,
                            ShippingRate = shippingRate,
                            ShippingMethodName = shippingMethod.Name,
                            ShippingMethod = shippingMethod.ToKeyResourceIdentifier(),
                            TaxCategory = taxCategory.ToKeyResourceIdentifier()
                        };
                        var orderImportDraft = DefaultOrderImportDraftWithLineItemByProductId(new OrderImportDraft(), product.Id);
                        orderImportDraft.ShippingInfo = shippingInfoImportDraft;

                        //Act
                        var importOrderCommand = new ImportOrderCommand(orderImportDraft);
                        var order = await client.ExecuteAsync(importOrderCommand);

                        //Assert
                        Assert.Null(order.Cart);
                        Assert.NotNull(order.ShippingInfo);
                        var shippingInfo = order.ShippingInfo;
                        Assert.Equal(orderImportDraft.ShippingInfo.Price, shippingInfo.Price);
                        Assert.Equal(orderImportDraft.ShippingInfo.ShippingMethodName, shippingInfo.ShippingMethodName);

                        Assert.Equal(shippingMethod.Id, shippingInfo.ShippingMethod.Id);
                        Assert.Equal(taxCategory.Id, shippingInfo.TaxCategory.Id);
                        Assert.Equal(shippingRate.Price, order.ShippingInfo.ShippingRate.Price);
                        Assert.Equal(shippingRate.FreeAbove, order.ShippingInfo.ShippingRate.FreeAbove);

                        //Delete it
                        await DeleteResource(client, order);
                    });
                });
            });

        }

        [Fact]
        public async Task TestImportOrderWithCustomLineItem()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                //Arrange
                var customLineItemDraft = new CustomLineItemDraft
                {
                    Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}},
                    Slug = TestingUtility.RandomString(10),
                    Quantity = TestingUtility.RandomInt(1,10),
                    Money = Money.FromDecimal("EUR", TestingUtility.RandomInt(100,10000)),
                    TaxCategory = taxCategory.ToKeyResourceIdentifier()
                };
                var orderImportDraft = DefaultOrderImportDraft(new OrderImportDraft());
                orderImportDraft.CustomLineItems = new List<CustomLineItemDraft>{ customLineItemDraft };

                //Act
                var importOrderCommand = new ImportOrderCommand(orderImportDraft);
                var order = await client.ExecuteAsync(importOrderCommand);

                //Assert
                Assert.Null(order.Cart);
                var orderCustomLineItem = order.CustomLineItems.FirstOrDefault();
                Assert.NotNull(orderCustomLineItem);
                Assert.Equal(customLineItemDraft.Name, orderCustomLineItem.Name);
                Assert.Equal(customLineItemDraft.Slug, orderCustomLineItem.Slug);
                Assert.Equal(customLineItemDraft.Quantity, orderCustomLineItem.Quantity);
                Assert.Equal(customLineItemDraft.Money, orderCustomLineItem.Money);
                Assert.Equal(taxCategory.Id, orderCustomLineItem.TaxCategory.Id);
                //Delete it
                await DeleteResource(client, order);
            });

        }
    }
}
