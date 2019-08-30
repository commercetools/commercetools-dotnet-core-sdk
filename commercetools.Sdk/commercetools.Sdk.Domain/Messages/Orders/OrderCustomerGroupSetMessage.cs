using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderCustomerGroupSet")]
    public class OrderCustomerGroupSetMessage : Message<Order>
    {
        public Reference<CustomerGroup> CustomerGroup { get; set; }
        public Reference<CustomerGroup> OldCustomerGroup { get; set; }
    }
}
