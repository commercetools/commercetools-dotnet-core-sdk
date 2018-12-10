using System;

namespace commercetools.Sdk.Domain.Payments
{
    public class TransactionDraft
    {
        public DateTime Timestamp { get; set; }
        public TransactionType Type { get; set; }
        public Money Amount { get; set; }
        public string InteractionId { get; set; }
        public TransactionState State { get; set; }
    }
}