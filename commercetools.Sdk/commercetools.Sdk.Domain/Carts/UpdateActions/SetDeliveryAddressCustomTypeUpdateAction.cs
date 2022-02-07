namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetDeliveryAddressCustomTypeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setDeliveryAddressCustomType";
        [Required]
        public string DeliveryId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}