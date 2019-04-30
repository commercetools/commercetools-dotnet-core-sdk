using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Domain.Carts
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class CartDraft : IDraft<Cart>
    {
        [Required]
        [Currency]
        public string Currency { get; set; }

        public string CustomerId { get; set; }

        public string CustomerEmail { get; set; }

        public IReference<CustomerGroup> CustomerGroup { get; set; }

        public string AnonymousId { get; set; }

        [Country]
        public string Country { get; set; }

        public InventoryMode InventoryMode { get; set; }

        public TaxMode TaxMode { get; set; }

        public RoundingMode TaxRoundingMode { get; set; }

        public TaxCalculationMode TaxCalculationMode { get; set; }

        public List<LineItemDraft> LineItems { get; set; }

        public List<CustomLineItemDraft> CustomLineItems { get; set; }

        public Address ShippingAddress { get; set; }

        public Address BillingAddress { get; set; }

        public Reference<ShippingMethod> ShippingMethod { get; set; }

        public ExternalTaxRateDraft ExternalTaxRateForShippingMethod { get; set; }

        public CustomFields Custom { get; set; }

        [Language]
        public string Locale { get; set; }

        public int DeleteDaysAfterLastModification { get; set; }

        public IShippingRateInputDraft ShippingRateInput { get; set; }

        public CartOrigin Origin { get; set; }

        public List<Address> ItemShippingAddresses { get; set; }
    }
}
