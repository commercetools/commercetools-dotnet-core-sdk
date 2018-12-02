namespace commercetools.Sdk.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InventoryEntryDraft : IDraft<InventoryEntry>
    {
        [Required]
        public string Sku { get; set; }
        public Reference<Channel> SupplyChannel { get; set; }
        public double QuantityOnStock { get; set; }
        public int RestockableInDays { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public CustomFields Custom { get; set; }
    }
}
