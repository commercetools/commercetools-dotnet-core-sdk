namespace commercetools.Sdk.Domain.CustomerGroups.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<CustomerGroup>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}