using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace commercetools.Common
{
    /// <summary>
    /// List of project scopes used for access tokens.
    /// </summary>
    /// <remarks>
    /// The actual value used for API requests is stored in the EnumMember attribute.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-authorization.html#scopes"/>
    [DataContract]
    public enum ProjectScope
    {
        [EnumMember(Value="manage_project")]
        ManageProject,
        [EnumMember(Value="manage_products")]
        ManageProducts,
        [EnumMember(Value="view_products")]
        ViewProducts,
        [EnumMember(Value="manage_orders")]
        ManageOrders,
        [EnumMember(Value="view_orders")]
        ViewOrders,
        [EnumMember(Value = "manage_my_orders")]
        ManageMyOrders,
        [EnumMember(Value="manage_customers")]
        ManageCustomers,
        [EnumMember(Value="view_customers")]
        ViewCustomers,
        [EnumMember(Value = "manage_my_profile")]
        ManageMyProfile,
        [EnumMember(Value="manage_types")]
        ManageTypes,
        [EnumMember(Value="view_types")]
        ViewTypes,
        [EnumMember(Value="manage_payments")]
        ManagePayments,
        [EnumMember(Value="view_payments")]
        ViewPayments,
        [EnumMember(Value = "create_anonymous_token")]
        CreateAnonymousToken
    }

    /// <summary>
    /// List of reference expansions.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api.html#reference-expansion"/>
    public enum ReferenceExpansion
    {
        Carts,
        CartDiscounts,
        Categories,
        Channels,
        Customers,
        CustomerGroups,
        DiscountCodes,
        State,
        Inventory,
        Messages,
        Orders,
        Products,
        ProductDiscounts,
        ProductProjections,
        ProductTypes,
        Reviews,
        ShippingMethods,
        TaxCategories,
        Types,
        Zone
    }

    /// <summary>
    /// List of reference types.
    /// </summary>
    /// <remarks>
    /// The actual value used for API requests is stored in the Description attribute.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-types.html#reference-types"/>
    [DataContract]
    public enum ReferenceType
    {
        [EnumMember(Value="cart")]
        Cart,
        [EnumMember(Value="cart-discount")]
        CartDiscount,
        [EnumMember(Value="category")]
        Category,
        [EnumMember(Value="channel")]
        Channel,
        [EnumMember(Value="customer")]
        Customer,
        [EnumMember(Value="customer-group")]
        CustomerGroup,
        [EnumMember(Value="discount-code")]
        DiscountCode,
        [EnumMember(Value="key-value-document")]
        KeyValueDocument,
        [EnumMember(Value="payment")]
        Payment,
        [EnumMember(Value="product")]
        Product,
        [EnumMember(Value="product-discount")]
        ProductDiscount,
        [EnumMember(Value="product-type")]
        ProductType,
        [EnumMember(Value="order")]
        Order,
        [EnumMember(Value="shipping-method")]
        ShippingMethod,
        [EnumMember(Value="state")]
        State,
        [EnumMember(Value="tax-category")]
        TaxCategory,
        [EnumMember(Value="type")]
        Type,
        [EnumMember(Value="zone")]
        Zone
    }
}
