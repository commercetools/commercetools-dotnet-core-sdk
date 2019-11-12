using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.InventoryEntries.UpdateActions
{
    public class ChangeQuantityUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "changeQuantity";
        [Required]
        public long Quantity { get; set; }
    }
}