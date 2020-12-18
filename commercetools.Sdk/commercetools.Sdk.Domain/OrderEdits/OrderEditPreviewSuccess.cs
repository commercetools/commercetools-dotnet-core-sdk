using System.Collections.Generic;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.Messages.Orders;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits
{
    [TypeMarker("PreviewSuccess")]
    public class OrderEditPreviewSuccess : OrderEditResult
    {
        public Order Preview { get; set; }
        
        public List<Message> MessagePayloads { get; set; }
    }
}