namespace commercetools.Sdk.Domain.InventoryEntries.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}
