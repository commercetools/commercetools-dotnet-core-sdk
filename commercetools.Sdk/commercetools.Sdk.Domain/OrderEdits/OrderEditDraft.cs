using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits
{
    public class OrderEditDraft : IDraft<OrderEdit>
    {
        public string Key { get; set; }
        
        public IReference<Order> Resource { get; set; }
        
        public CustomFieldsDraft Custom { get; set; }

        public string Comment { get; set; }

        public bool? DryRun { get; set; }

        public List<IStagedOrderUpdateAction> StagedActions { get; set; }
    }
}