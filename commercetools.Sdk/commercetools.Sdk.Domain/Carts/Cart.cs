namespace commercetools.Sdk.Domain.Carts
{
    using System;
    using System.Collections.Generic;

    public class Cart
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string AnonymousId { get; set; }
        public List<LineItem> LineItems { get; set; }
        public List<CustomLineItem> CustomLineItems { get; set; }
        public Money TotalPrice { get; set; }
        public TaxedPrice TaxedPrice { get; set; }
        public CartState CartState { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public InventoryMode InventoryMode { get; set; }
        public TaxMode TaxMode { get; set; }
        public RoundingMode TaxRoundingMode { get; set; }
        public TaxCalculationMode TaxCalculationMode { get; set; }
        public Reference<CustomerGroup> CustomerGroup { get; set; }
        public string Country { get; set; }
        public ShippingInfo ShippingInfo { get; set; }
        public List<DiscountCodeInfo> DiscountCodes { get; set; }
        public List<Reference<CartDiscount>> RefusedGifts { get; set; }
        public CustomFields Custom { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public string Locale { get; set; }
        public int DeleteDaysAfterLastModification { get; set; }
        public IShippingRateInput ShippingRateInput { get; set; }
        public CartOrigin Origin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public List<Address> ItemShippingAddresses { get; set; }
        public ItemShippingDetails ShippingDetails { get; set; }
    }
}