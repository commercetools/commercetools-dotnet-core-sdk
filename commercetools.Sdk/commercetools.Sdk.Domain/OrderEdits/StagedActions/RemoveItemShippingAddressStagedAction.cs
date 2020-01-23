namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveItemShippingAddressStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "removeItemShippingAddress";
        [Required]
        public string AddressKey { get; set; }
    }
}