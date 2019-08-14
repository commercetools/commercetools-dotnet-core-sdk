using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.IntegrationTests.OrdersImport
{
    public static class OrdersImportFixture
    {
        #region DraftBuilds

        public static OrderImportDraft DefaultOrderImportDraft(OrderImportDraft orderImportDraft)
        {
            var amountEuro10 = Money.FromDecimal("EUR", 10);
            var taxedPrice = TestingUtility.GetTaxedPrice(amountEuro10, 0.19);
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
            var priceEuro10 = TestingUtility.GetPriceFromDecimal(10);
            var variant = new ProductVariantImportDraft(productId, 1);

            var addressKey = draft.ItemShippingAddresses.FirstOrDefault()?.Key;
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
                ProductId = productId,
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
            orderImportDraft.LineItems = new List<LineItemImportDraft>
            {
                lineItemImportDraft
            };
            return orderImportDraft;
        }

        #endregion
    }
}
