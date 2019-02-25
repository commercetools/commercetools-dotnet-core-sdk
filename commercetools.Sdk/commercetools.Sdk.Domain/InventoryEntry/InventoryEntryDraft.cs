using commercetools.Sdk.Domain.Channels;

namespace commercetools.Sdk.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InventoryEntryDraft : IDraft<InventoryEntry>
    {
        [Required]
        public string Sku { get; set; }
        public Reference<Channel> SupplyChannel { get; set; }
        public long QuantityOnStock { get; set; }
        public int RestockableInDays { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}
