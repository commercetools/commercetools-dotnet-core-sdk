using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class AddDeliveryUpdateAction : OrderUpdateAction
    {
        public override string Action => "addDelivery";
        public List<DeliveryItem> Items { get; set; }
        public Address Address { get; set; }
        public List<ParcelDraft> Parcels { get; set; }
    }
}