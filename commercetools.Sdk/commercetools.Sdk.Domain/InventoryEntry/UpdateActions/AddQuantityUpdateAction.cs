using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.InventoryEntries
{
    public class AddQuantityUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "addQuantity";
        [Required]
        public long Quantity { get; set; }
    }
}