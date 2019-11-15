using System.ComponentModel;

namespace commercetools.Sdk.Domain.Types
{
    public enum ReferenceFieldTypeId
    {
        [Description("product")]
        Product,

        [Description("product-type")]
        ProductType,

        [Description("channel")]
        Channel,

        [Description("customer")]
        Customer,

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