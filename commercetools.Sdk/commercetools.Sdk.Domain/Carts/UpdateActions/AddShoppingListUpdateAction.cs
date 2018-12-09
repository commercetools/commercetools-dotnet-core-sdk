namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddShoppingListUpdateAction : UpdateAction<Cart>
    {
        public string Action => "addShoppingList";
        [Required]
        public ResourceIdentifier ShoppingList { get; set; }
        public ResourceIdentifier SupplyChannel { get; set; }
        public ResourceIdentifier DistributionChannel { get; set; }

    }
}