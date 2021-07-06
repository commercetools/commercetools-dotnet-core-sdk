using System.Collections.Generic;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Common
{
    public interface IShopping
    {
        string CustomerId { get; set; }
        string CustomerEmail { get; set; }
        string AnonymousId { get; set; }
        KeyReference<Store> Store { get; set; }
        List<LineItem> LineItems { get; set; }
        List<CustomLineItem> CustomLineItems { get; set; }
        Money TotalPrice { get; set; }
        TaxedPrice TaxedPrice { get; set; }
        Address ShippingAddress { get; set; }
        Address BillingAddress { get; set; }
        TaxMode TaxMode { get; set; }
        RoundingMode TaxRoundingMode { get; set; }
        TaxCalculationMode TaxCalculationMode { get; set; }
        Reference<CustomerGroup> CustomerGroup { get; set; }
        string Country { get; set; }
        ShippingInfo ShippingInfo { get; set; }
        List<DiscountCodeInfo> DiscountCodes { get; set; }
        List<Reference<CartDiscount>> RefusedGifts { get; set; }
        CustomFields Custom { get; set; }
        PaymentInfo PaymentInfo { get; set; }
        string Locale { get; set; }
        IShippingRateInput ShippingRateInput { get; set; }
        CartOrigin Origin { get; set; }
        List<Address> ItemShippingAddresses { get; set; }
    }
}