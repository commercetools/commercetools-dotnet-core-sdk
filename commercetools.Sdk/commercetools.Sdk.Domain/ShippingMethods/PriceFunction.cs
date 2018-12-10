using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.ShippingMethods
{
    public class PriceFunction
    {
        [Currency]
        public string CurrencyCode { get; set; }
        // TODO See if a predicate expression should be made for this
        public string Function { get; set; }
    }
}