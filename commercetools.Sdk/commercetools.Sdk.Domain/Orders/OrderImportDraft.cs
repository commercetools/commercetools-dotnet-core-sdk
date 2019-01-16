using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Orders
{
    public class OrderImportDraft : IImportDraft<Order>
    {
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public List<LineItemImportDraft> LineItems { get; set; }
        public List<CustomLineItem> CustomLineItems { get; set; }
        [Required]
        public Money TotalPrice { get; set; }
        public TaxedPrice TaxedPrice { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public Reference<CustomerGroup> CustomerGroup { get; set; }
        [Country]
        public string Country { get; set; }
        public OrderState OrderState { get; set; }
        public ShipmentState ShipmentState { get; set; }
        public ShippingInfo ShippingInfo { get; set; }
        public DateTime CompletedAt { get; set; }
        public CustomFieldsDraft Custom { get; set; }
        public InventoryMode InventoryMode { get; set; }
        public RoundingMode TaxRoundingMode { get; set; }
        public TaxCalculationMode TaxCalculationMode { get; set; }
        public CartOrigin Origin { get; set; }
        public List<Address> ItemShippingAddresses { get; set; }
    }
}
