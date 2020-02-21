using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Orders
{
    public class OrderFromCartDraft : IDraft<Order>
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string OrderNumber { get; set; }
        public OrderState OrderState { get; set; }
        public Reference<State> State { get; set; }
        public ShipmentState? ShipmentState { get; set; }
        public PaymentState? PaymentState { get; set; }
    }
}
