using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.Carts
{
    public class Delivery
    {
        public string Id { get; set;}

        public DateTime CreatedAt { get; set;}

        public List<DeliveryItem> Items { get; set;}

        public List<Parcel> Parcels { get; set;}

        public Address Address { get; set;}
    }
}
