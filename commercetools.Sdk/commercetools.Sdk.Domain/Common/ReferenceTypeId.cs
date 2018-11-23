using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public enum ReferenceTypeId
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
