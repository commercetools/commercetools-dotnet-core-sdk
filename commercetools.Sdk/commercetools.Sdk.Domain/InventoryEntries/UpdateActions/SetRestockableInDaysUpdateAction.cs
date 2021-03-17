namespace commercetools.Sdk.Domain.InventoryEntries.UpdateActions
{
    public class SetRestockableInDaysUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setRestockableInDays";
        public int? RestockableInDays { get; set; }
    }
}