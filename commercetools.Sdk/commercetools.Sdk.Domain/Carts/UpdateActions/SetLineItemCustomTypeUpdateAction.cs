namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemCustomTypeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setLineItemCustomType";
        [Required]
        public string LineItemId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}