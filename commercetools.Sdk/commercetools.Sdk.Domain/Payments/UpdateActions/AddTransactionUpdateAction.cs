using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class AddTransactionUpdateAction : UpdateAction<Payment>
    {
        public string Action => "addTransaction";
        [Required]
        public TransactionDraft Transaction { get; set; }
    }
}