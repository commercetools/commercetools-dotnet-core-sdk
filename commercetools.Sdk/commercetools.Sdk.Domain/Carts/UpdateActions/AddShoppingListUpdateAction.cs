using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class AddShoppingListUpdateAction : CartUpdateAction
    {
        public override string Action => "addShoppingList";
        [Required]
        public ResourceIdentifier ShoppingList { get; set; }
        public ResourceIdentifier SupplyChannel { get; set; }
        public ResourceIdentifier DistributionChannel { get; set; }

    }
}