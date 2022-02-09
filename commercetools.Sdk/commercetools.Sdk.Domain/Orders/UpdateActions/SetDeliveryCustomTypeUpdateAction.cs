using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetDeliveryCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setDeliveryCustomType";
        [Required] 
        public string DeliveryId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}