using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetTransactionCustomFieldUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setTransactionCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
        [Required]
        public string TransactionId { get; set; }
    }
}