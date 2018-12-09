namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetCustomLineItemCustomTypeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCustomLineItemCustomType";
        [Required]
        public string CustomLineItemId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}