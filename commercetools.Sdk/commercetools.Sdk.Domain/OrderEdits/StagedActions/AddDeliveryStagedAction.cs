using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class AddDeliveryStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "addDelivery";
        public List<DeliveryItem> Items { get; set; }
        public Address Address { get; set; }
        public List<ParcelDraft> Parcels { get; set; }
    }
}