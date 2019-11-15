namespace commercetools.Sdk.Domain.InventoryEntries.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}