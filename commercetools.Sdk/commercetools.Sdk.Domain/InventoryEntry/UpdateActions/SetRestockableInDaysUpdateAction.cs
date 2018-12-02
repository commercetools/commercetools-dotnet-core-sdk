using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.InventoryEntries
{
    public class SetRestockableInDaysUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setRestockableInDays";
        public int RestockableInDays { get; set; }
    }
}