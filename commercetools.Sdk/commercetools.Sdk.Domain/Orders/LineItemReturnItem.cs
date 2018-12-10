using System;

namespace commercetools.Sdk.Domain.Orders
{
    [TypeMarker("LineItemReturnItem")]
    public class LineItemReturnItem : ReturnItem
    {
        public string LineItemId { get; set; }
    }
}