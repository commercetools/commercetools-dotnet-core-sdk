using System.ComponentModel;

namespace commercetools.Sdk.Domain.APIExtensions
{
    public enum ExtensionResourceType
    {
        [Description("cart")]
        Cart,

        [Description("customer")]
        Customer,

        [Description("payment")]
        Payment,

        [Description("order")]
        Order
    }
}
