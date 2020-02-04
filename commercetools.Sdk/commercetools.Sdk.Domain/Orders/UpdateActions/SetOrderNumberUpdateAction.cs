using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetOrderNumberUpdateAction : OrderUpdateAction
    {
        public override string Action => "setOrderNumber";
        public string OrderNumber { get; set; }
    }
}