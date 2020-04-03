using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Stores;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderStoreSet")]
    public class OrderStoreSetMessage : Message<Order>
    {
        public KeyReference<Store> Store { get; set; }
    }
}
