using System.Collections.Generic;
using commercetools.Sdk.Domain.Payments;

namespace commercetools.Sdk.Domain.Orders
{
    public class PaymentInfo
    {
        public List<Reference<Payment>> Payments { get; set; }
    }
}