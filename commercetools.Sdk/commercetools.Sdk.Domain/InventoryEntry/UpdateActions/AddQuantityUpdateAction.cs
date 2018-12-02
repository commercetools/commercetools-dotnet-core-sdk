using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.InventoryEntries
{
    public class AddQuantityUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "addQuantity";
        [Required]
        public double Quantity { get; set; }
    }
}