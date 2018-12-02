using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.InventoryEntries
{
    public class RemoveQuantityUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "removeQuantity";
        [Required]
        public double Quantity { get; set; }
    }
}