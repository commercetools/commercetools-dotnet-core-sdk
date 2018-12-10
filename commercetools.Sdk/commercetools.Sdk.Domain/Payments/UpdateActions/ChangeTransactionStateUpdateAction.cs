using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class ChangeTransactionStateUpdateAction : UpdateAction<Payment>
    {
        public string Action => "changeTransactionState";
        [Required]
        public Guid TransactionId { get; set; }
        [Required]
        public TransactionState State { get; set; }
    }
}