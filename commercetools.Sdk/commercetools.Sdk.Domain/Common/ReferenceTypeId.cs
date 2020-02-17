using System.ComponentModel;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.Domain.Zones;

namespace commercetools.Sdk.Domain
{
    public enum ReferenceTypeId
    {
        [Description("cart")]
        Cart,

        [Description("cart-discount")]
        CartDiscount,

        [Description("category")]
        Category,

        [Description("channel")]
        Channel,

        [Description("customer")]
        Customer,

        [Description("customer-group")]
        CustomerGroup,

        [Description("discount-code")]
        DiscountCode,

        [Description("key-value-document")]
        KeyValueDocument,

        [Description("payment")]
        Payment,

        [Description("product")]
        Product,

        [Description("product-discount")]
        ProductDiscount,

        [Description("product-price")]
        ProductPrice,

        [Description("product-type")]
        ProductType,

        [Description("order")]
        Order,

        [Description("shipping-method")]
        ShippingMethod,

        [Description("shopping-list")]
        ShoppingList,

        [Description("state")]
        State,

        [Description("tax-category")]
        TaxCategory,

        [Description("type")]
        Type,

        [Description("zone")]
        Zone,

        [Description("store")]
        Store,

        [Description("review")]
        Review,

        [Description("subscription")]
        Subscription,

        [Description("inventory-entry")]
        InventoryEntry,
        
        [Description("order-edit")]
        OrderEdit
    }
}
