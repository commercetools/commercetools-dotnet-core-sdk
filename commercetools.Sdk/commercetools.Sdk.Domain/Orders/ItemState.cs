using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Orders
{
    public class ItemState
    {
        public long Quantity { get; set; }
        public Reference<State> State { get; set; }
    }
}
