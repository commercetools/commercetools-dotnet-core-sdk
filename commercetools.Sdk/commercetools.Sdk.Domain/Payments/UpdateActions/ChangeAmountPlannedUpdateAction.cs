using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class ChangeAmountPlannedUpdateAction : UpdateAction<Payment>
    {
        public string Action => "changeAmountPlanned";
        [Required]
        public Money AmountPlanned { get; set; }
    }
}