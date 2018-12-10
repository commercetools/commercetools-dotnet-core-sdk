using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetOrderNumberUpdateAction : UpdateAction<Order>
    {
        public string Action => "setOrderNumber";
        public string OrderNumber { get; set; }
    }
}