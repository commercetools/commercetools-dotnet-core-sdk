using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.InventoryEntries.UpdateActions
{
    public class RemoveQuantityUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "removeQuantity";
        [Required]
        public long Quantity { get; set; }
    }
}