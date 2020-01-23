namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateItemShippingAddressStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "updateItemShippingAddress";
        [Required]
        public Address Address { get; set; }
    }
}