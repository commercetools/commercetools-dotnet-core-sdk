using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Orders
{
    public class ItemState
    {
        public double Quantity { get; set; }
        public Reference<State> State { get; set; }
    }
}