using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class RemovePaymentStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "removePayment";
        [Required]
        public IReference<Payment> Payment { get; set; }
    }
}