namespace commercetools.Sdk.Domain
{
    using System.ComponentModel;

    public enum ReferenceAttributeTypeId
    {
        [Description("product")]
        Product,

        [Description("product-type")]
        ProductType,

        [Description("channel")]
        Channel,

        [Description("state")]
        State,

        [Description("zone")]
        Zone,

        [Description("shipping-method")]
        ShippingMethod,

        [Description("category")]
        Category,

        [Description("review")]
        Review,

        [Description("key-value-document")]
        KeyValueDocument
    }
}