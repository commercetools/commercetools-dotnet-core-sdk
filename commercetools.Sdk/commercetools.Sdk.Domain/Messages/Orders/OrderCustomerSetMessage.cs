using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Orders;


namespace commercetools.Sdk.Domain.Messages.Orders
{
    [TypeMarker("OrderCustomerSet")]
    public class OrderCustomerSetMessage : Message<Order>
    {
        public Reference<Customer> Customer { get; set; }
        public Reference<CustomerGroup> CustomerGroup { get; set; }
        public Reference<Customer> OldCustomer { get; set; }
        public Reference<CustomerGroup> OldCustomerGroup { get; set; }
    }
}
