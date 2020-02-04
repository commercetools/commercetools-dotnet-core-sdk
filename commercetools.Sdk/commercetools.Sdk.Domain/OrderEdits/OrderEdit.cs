using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits
{
    [Endpoint("orders/edits")]
    public class OrderEdit: Resource<OrderEdit>, IKeyReferencable<OrderEdit>
    {
        public string Key { get; set; }
        
        public Reference<Order> Resource { get; set; }
        
        public CustomFields Custom { get; set; }

        public string Comment { get; set; }
        
        public List<IStagedOrderUpdateAction> StagedActions { get; set; }

        public OrderEditResult Result { get; set; }
    }
}