namespace commercetools.Sdk.Domain.Payments
{
    public class PaymentMethodInfo
    {
        public string PaymentInterface { get; set; }
        public string Method { get; set; }
        public LocalizedString Name { get; set; }
    }
}