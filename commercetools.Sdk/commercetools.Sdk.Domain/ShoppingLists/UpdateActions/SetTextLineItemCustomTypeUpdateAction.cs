namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetTextLineItemCustomTypeUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setTextLineItemCustomType";
        [Required]
        public string TextLineItemId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}