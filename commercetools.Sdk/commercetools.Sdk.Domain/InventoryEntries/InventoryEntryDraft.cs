using System;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.InventoryEntries
{
    public class InventoryEntryDraft : IDraft<InventoryEntry>
    {
        [Required]
        public string Sku { get; set; }
        public IReference<Channel> SupplyChannel { get; set; }
        public long QuantityOnStock { get; set; }
        public int RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}
