using commercetools.Sdk.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Payments
{
    public class PaymentDraft : IDraft<Payment>
    {
        public string Key { get; set; }
        public Reference<Customer> Customer { get; set; }
        public string AnonymousId { get; set; }
        public string InterfaceId { get; set; }
        public Money AmountPlanned { get; set; }
        public PaymentMethodInfo PaymentMethodInfo { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<TransactionDraft> Transactions { get; set; }
        public List<CustomFieldsDraft> InterfaceInteractions { get; set; }
        public CustomFields Custom { get; set; }
    }
}
