using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class RemoveDeliveryUpdateAction : OrderUpdateAction
    {
        public override string Action => "removeDelivery";
        [Required]
        public string DeliveryId { get; set; }
    }
}