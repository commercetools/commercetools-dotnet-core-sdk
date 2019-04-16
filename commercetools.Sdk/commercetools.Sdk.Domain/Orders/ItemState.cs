using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Orders
{
    public class ItemState
    {
        public double Quantity { get; set; }
        public Reference<State> State { get; set; }
    }
}