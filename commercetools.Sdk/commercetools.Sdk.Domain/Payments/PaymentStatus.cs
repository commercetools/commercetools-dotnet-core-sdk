namespace commercetools.Sdk.Domain.Payments
{
    public class PaymentStatus
    {
        public string InterfaceCode { get; set; }
        public string InterfaceText { get; set; }
        public Reference<State> State { get; set; }
    }
}