using System.ComponentModel.DataAnnotations;
namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class AddDiscountCodeStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "addDiscountCode";
        [Required]
        public string Code { get; set; }
    }
}