namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    using System.ComponentModel.DataAnnotations;

    public class AddItemShippingAddressStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "addItemShippingAddress";
        [Required]
        public Address Address { get; set; }
    }
}