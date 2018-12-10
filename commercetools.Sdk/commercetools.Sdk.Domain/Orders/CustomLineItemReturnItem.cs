using System;

namespace commercetools.Sdk.Domain.Orders
{
    [TypeMarker("CustomLineItemReturnItem")]
    public class CustomLineItemReturnItem : ReturnItem
    {
        public string CustomLineItemId { get; set; }
    }
}