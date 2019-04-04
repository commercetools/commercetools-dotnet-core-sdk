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
        [ResourceTypeMarker(typeof(Cart))]
        Cart,

        [Description("cart-discount")]
        [ResourceTypeMarker(typeof(CartDiscount))]
        CartDiscount,

        [Description("category")]
        [ResourceTypeMarker(typeof(Category))]
        Category,

        [Description("channel")]
        [ResourceTypeMarker(typeof(Channel))]
        Channel,

        [Description("customer")]
        [ResourceTypeMarker(typeof(Customer))]
        Customer,

        [Description("customer-group")]
        [ResourceTypeMarker(typeof(CustomerGroup))]
        CustomerGroup,

        [Description("discount-code")]
        [ResourceTypeMarker(typeof(DiscountCode))]
        DiscountCode,

        [Description("key-value-document")]
        KeyValueDocument,

        [Description("payment")]
        [ResourceTypeMarker(typeof(Payment))]
        Payment,

        [Description("product")]
        [ResourceTypeMarker(typeof(Product))]
        Product,

        [Description("product-discount")]
        [ResourceTypeMarker(typeof(ProductDiscount))]
        ProductDiscount,

        [Description("product-price")]
        [ResourceTypeMarker(typeof(Price))]
        ProductPrice,

        [Description("product-type")]
        [ResourceTypeMarker(typeof(ProductType))]
        ProductType,

        [Description("order")]
        [ResourceTypeMarker(typeof(Order))]
        Order,

        [Description("shipping-method")]
        [ResourceTypeMarker(typeof(ShippingMethod))]
        ShippingMethod,

        [Description("shipping-list")]
        [ResourceTypeMarker(typeof(ShoppingList))]
        ShoppingList,

        [Description("state")]
        [ResourceTypeMarker(typeof(State))]
        State,

        [Description("tax-category")]
        [ResourceTypeMarker(typeof(TaxCategory))]
        TaxCategory,

        [Description("type")]
        [ResourceTypeMarker(typeof(Type))]
        Type,

        [Description("zone")]
        [ResourceTypeMarker(typeof(Zone))]
        Zone
    }
}
