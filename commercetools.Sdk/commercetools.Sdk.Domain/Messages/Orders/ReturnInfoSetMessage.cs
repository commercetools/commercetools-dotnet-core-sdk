using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("ReturnInfoSet")]
    public class ReturnInfoSetMessage : Message<Order>
    {
        public List<ReturnInfo> ReturnInfo { get; set; }
    }
}
