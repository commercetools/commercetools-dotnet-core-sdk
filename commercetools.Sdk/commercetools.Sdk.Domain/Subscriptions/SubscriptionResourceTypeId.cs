using System.ComponentModel;

namespace commercetools.Sdk.Domain.Subscriptions
{
    public enum SubscriptionResourceTypeId
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
        
        [Description("extension")]
        Extension,
        
        [Description("inventory-entry")]
        InventoryEntry,
        
        [Description("order")]
        Order,
        
        [Description("payment")]
        Payment,

        [Description("product")]
        Product,

        [Description("product-discount")]
        ProductDiscount,
        
        [Description("product-type")]
        ProductType,
        
        [Description("review")]
        Review,
        
        [Description("shopping-list")]
        ShoppingList,
        
        [Description("subscription")]
        Subscription,
        
        [Description("state")]
        State,
        
        [Description("tax-category")]
        TaxCategory,

        [Description("type")]
        Type,
        
        Unknown,

    }
}