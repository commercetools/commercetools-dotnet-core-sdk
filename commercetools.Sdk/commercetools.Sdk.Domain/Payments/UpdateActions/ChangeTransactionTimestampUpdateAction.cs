using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class ChangeTransactionTimestampUpdateAction : UpdateAction<Payment>
    {
        public string Action => "changeTransactionTimestamp";
        [Required]
        public Guid TransactionId { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
    }
}