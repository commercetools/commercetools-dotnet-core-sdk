using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.TaxCategories;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.OrdersImport
{
    public static class OrdersImportFixture
    {
        #region DraftBuilds

        public static OrderImportDraft DefaultOrderImportDraft(OrderImportDraft orderImportDraft)
        {
            var amountEuro10 = Money.FromDecimal("EUR", 10);
            var taxedPrice = TestingUtility.GetTaxedPrice(amountEuro10, 0.19m);
            var shippingAddress = TestingUtility.GetRandomAddress();
            var billingAddress = TestingUtility.GetRandomAddress();
            var itemShippingAddress = TestingUtility.GetRandomAddress();

            orderImportDraft.TotalPrice = amountEuro10;
            orderImportDraft.OrderState = OrderState.Open;
            orderImportDraft.Country = "DE";
            orderImportDraft.OrderNumber = TestingUtility.RandomString();
            orderImportDraft.PaymentState = PaymentState.Failed;
            orderImportDraft.ShipmentState = ShipmentState.Shipped;
            orderImportDraft.BillingAddress = billingAddress;
            orderImportDraft.ShippingAddress = shippingAddress;
            orderImportDraft.TaxedPrice = taxedPrice;
            orderImportDraft.ItemShippingAddresses = new List<Address>()
            {
                itemShippingAddress
            };
            orderImportDraft.InventoryMode = InventoryMode.None;
            orderImportDraft.TaxRoundingMode = RoundingMode.HalfUp;
            orderImportDraft.TaxCalculationMode = TaxCalculationMode.LineItemLevel;

            return orderImportDraft;
        }

        public static OrderImportDraft DefaultOrderImportDraftWithLineItemByProductId(OrderImportDraft draft,string productId)
        {
            var orderImportDraft = DefaultOrderImportDraft(draft);
            var variant = new ProductVariantImportDraft(productId, 1);

            var addressKey = draft.ItemShippingAddresses.FirstOrDefault()?.Key;
            var lineItemImportDraft = GetLineItemImportDraft(variant, addressKey);
            orderImportDraft.LineItems = new List<LineItemImportDraft>
            {
                lineItemImportDraft
            };
            return orderImportDraft;
        }

        public static OrderImportDraft DefaultOrderImportDraftWithLineItemBySku(OrderImportDraft draft,string sku)
        {
            var orderImportDraft = DefaultOrderImportDraft(draft);
            var variant = new ProductVariantImportDraft(sku);

            var addressKey = draft.ItemShippingAddresses.FirstOrDefault()?.Key;
            var lineItemImportDraft = GetLineItemImportDraft(variant, addressKey);
            orderImportDraft.LineItems = new List<LineItemImportDraft>
            {
                lineItemImportDraft
            };
            return orderImportDraft;
        }

        private static LineItemImportDraft GetLineItemImportDraft(ProductVariantImportDraft variant, string addressKey)
        {
            var priceEuro10 = TestingUtility.GetPriceFromDecimal(10);

            var lineItemImportDraft = new LineItemImportDraft
            {
                Variant = variant,
                Quantity = 2,
                Price = priceEuro10,
                Name = new LocalizedString
                {
                    {"en", "a name"},
                    {"de", "der Name"}
                },
                ProductId = variant.ProductId,
                ShippingDetails = new ItemShippingDetailsDraft()
                {
                    Targets = new List<ItemShippingTarget>()
                    {
                        new ItemShippingTarget()
                        {
                            Quantity = 2,
                            AddressKey = addressKey
                        }
                    }
                }
            };
            return lineItemImportDraft;
        }


        public static OrderImportDraft DefaultOrderImportDraftWithLineItemWithShippingInfo(OrderImportDraft draft,string productId, TaxCategory taxCategory, ShippingMethod shippingMethod)
        {
            var orderImportDraft = DefaultOrderImportDraftWithLineItemByProductId(draft, productId);
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
            orderImportDraft.ShippingInfo = shippingInfoImportDraft;
            return orderImportDraft;
        }
        public static OrderImportDraft DefaultOrderImportDraftWithCustomLineItem(OrderImportDraft draft,TaxCategory taxCategory)
        {
            var orderImportDraft = DefaultOrderImportDraft(draft);
            var customLineItemDraft = new CustomLineItemDraft
            {
                Name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}},
                Slug = TestingUtility.RandomString(10),
                Quantity = 100,
                Money = Money.FromDecimal("EUR", TestingUtility.RandomInt(100, 10000)),
                TaxCategory = taxCategory.ToKeyResourceIdentifier()
            };
            orderImportDraft.CustomLineItems = new List<CustomLineItemDraft> {customLineItemDraft};
            return orderImportDraft;
        }

        #endregion

        #region WithImportOrder

        public static async Task<Order> DoImportOrder(IClient client, IImportDraft<Order> buildDraft)
        {
            return await client
                .ExecuteAsync(new ImportOrderCommand(buildDraft));
        }

        public static async Task WithImportOrder( IClient client, Action<Order, OrderImportDraft> func)
        {
            await ImportWith(client, new OrderImportDraft(), DefaultOrderImportDraft, func, DoImportOrder);
        }
        public static async Task WithImportOrder( IClient client, Func<OrderImportDraft, OrderImportDraft> draftAction, Action<Order, OrderImportDraft> func)
        {
            await ImportWith(client, new OrderImportDraft(), draftAction, func, DoImportOrder);
        }

        public static async Task WithImportOrder( IClient client, Func<Order, OrderImportDraft, Task> func)
        {
            await ImportWithAsync(client, new OrderImportDraft(), DefaultOrderImportDraft, func, DoImportOrder);
        }
        public static async Task WithImportOrder( IClient client, Func<OrderImportDraft, OrderImportDraft> draftAction, Func<Order, OrderImportDraft, Task> func)
        {
            await ImportWithAsync(client, new OrderImportDraft(), draftAction, func, DoImportOrder);
        }
        #endregion
    }
}
