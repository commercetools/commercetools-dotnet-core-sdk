namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetCustomLineItemCustomFieldUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCustomLineItemCustomField";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}