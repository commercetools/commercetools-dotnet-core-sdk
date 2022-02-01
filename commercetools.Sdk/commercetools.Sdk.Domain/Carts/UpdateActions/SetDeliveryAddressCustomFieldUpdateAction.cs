namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetDeliveryAddressCustomFieldUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setDeliveryAddressCustomField";
        [Required]
        public string DeliveryId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}