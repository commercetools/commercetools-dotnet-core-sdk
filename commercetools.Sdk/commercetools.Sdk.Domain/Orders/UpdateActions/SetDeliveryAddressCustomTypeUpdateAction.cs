namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetDeliveryAddressCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setDeliveryAddressCustomType";
        [Required]
        public string DeliveryId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}