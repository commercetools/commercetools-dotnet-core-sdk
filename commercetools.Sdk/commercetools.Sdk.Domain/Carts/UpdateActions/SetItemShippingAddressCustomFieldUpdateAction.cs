namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetItemShippingAddressCustomFieldUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setItemShippingAddressCustomField";
        [Required]
        public string AddressKey { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}