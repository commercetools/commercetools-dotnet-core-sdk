namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetBillingAddressCustomFieldUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setBillingAddressCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}