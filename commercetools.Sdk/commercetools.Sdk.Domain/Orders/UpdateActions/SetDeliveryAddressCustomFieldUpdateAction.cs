namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetDeliveryAddressCustomFieldUpdateAction : OrderUpdateAction
    {
        public override string Action => "setDeliveryAddressCustomField";
        [Required]
        public string DeliveryId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}