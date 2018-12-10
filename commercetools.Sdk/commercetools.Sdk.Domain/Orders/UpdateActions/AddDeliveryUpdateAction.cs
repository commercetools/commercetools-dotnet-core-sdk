using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class AddDeliveryUpdateAction : UpdateAction<Order>
    {
        public string Action => "addDelivery";
        public List<DeliveryItem> Items { get; set; }
        public Address Address { get; set; }
        public List<ParcelDraft> Parcels { get; set; }
    }
}