namespace commercetools.Sdk.Domain.Carts
{
    [TypeMarker("Classification")]
    public class ClassificationShippingRateInput : ClassificationShippingRateInputBase, IShippingRateInput
    {
        public LocalizedString Label { get; set; }
    }
}
