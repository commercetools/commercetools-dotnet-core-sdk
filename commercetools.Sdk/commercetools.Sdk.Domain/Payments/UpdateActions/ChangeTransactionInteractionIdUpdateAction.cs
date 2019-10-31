using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class ChangeTransactionInteractionIdUpdateAction : UpdateAction<Payment>
    {
        public string Action => "changeTransactionInteractionId";
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public string InteractionId { get; set; }
    }
}