using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Orders
{
    public class Delivery
    {
        public string Id { get; set; }
        public DateTime CreateAt { get; set; }
        public List<DeliveryItem> Items { get; set; }
        public List<Parcel> Parcels { get; set; }
        public Address Address { get; set; }
    }
}