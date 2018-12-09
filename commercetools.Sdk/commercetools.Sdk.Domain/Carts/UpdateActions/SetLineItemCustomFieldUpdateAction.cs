namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemCustomFieldUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setLineItemCustomField";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}