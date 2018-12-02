namespace commercetools.Sdk.Domain.InventoryEntries
{
    public class SetCustomFieldUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setCustomField";
        public string Name { get; set; }
        public object Value { get; set; }
    }
}