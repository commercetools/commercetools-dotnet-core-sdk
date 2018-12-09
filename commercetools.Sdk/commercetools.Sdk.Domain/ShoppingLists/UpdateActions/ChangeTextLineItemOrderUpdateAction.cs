namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ChangeTextLineItemOrderUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "changeTextLineItemsOrder";
        [Required]
        public List<string> TextLineItemOrder { get; set; }
    }
}