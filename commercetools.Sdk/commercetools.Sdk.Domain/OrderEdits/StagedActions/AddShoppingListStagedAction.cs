using System.ComponentModel.DataAnnotations;
namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class AddShoppingListStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "addShoppingList";
        [Required]
        public ResourceIdentifier ShoppingList { get; set; }
        public ResourceIdentifier SupplyChannel { get; set; }
        public ResourceIdentifier DistributionChannel { get; set; }

    }
}