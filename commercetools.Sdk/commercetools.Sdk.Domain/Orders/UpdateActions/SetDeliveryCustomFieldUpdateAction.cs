using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetDeliveryCustomFieldUpdateAction : OrderUpdateAction
    {
        public override string Action => "setDeliveryCustomField";
        [Required]
        public string DeliveryId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}