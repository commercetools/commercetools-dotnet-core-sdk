using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
        [Description("shipping-list")]
        ShoppingList,
        [Description("state")]
        State,
        [Description("tax-category")]
        TaxCategory,
        [Description("type")]
        Type,
        [Description("zone")]
        Zone        
    }
}
