using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class AddPaymentStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "addPayment";
        [Required]
        public IReference<Payment> Payment { get; set; }
    }
}