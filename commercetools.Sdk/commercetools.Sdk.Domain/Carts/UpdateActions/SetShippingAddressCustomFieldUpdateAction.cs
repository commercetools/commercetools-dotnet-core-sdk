namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetShippingAddressCustomFieldUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setShippingAddressCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}