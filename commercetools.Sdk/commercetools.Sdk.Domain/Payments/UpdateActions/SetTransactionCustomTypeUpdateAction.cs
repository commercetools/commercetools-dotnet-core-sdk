using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetTransactionCustomTypeUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setTransactionCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
        [Required]
        public string TransactionId { get; set; }
    }
}