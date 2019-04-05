using commercetools.Sdk.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Payments
{
    [Endpoint("payments")]
    [ResourceType(ReferenceTypeId.Payment)]
    public class Payment
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Key { get; set; }
        public Reference<Customer> Customer { get; set; }
        public string AnonymousId { get; set; }
        public string InterfaceId { get; set; }
        public Money AmountPlanned { get; set; }
        public PaymentMethodInfo PaymentMethodInfo { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<CustomFields> InterfaceInteractions { get; set; }
        public CustomFields Custom { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
