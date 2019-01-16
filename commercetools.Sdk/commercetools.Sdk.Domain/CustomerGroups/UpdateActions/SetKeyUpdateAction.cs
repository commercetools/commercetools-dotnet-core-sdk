namespace commercetools.Sdk.Domain.CustomerGroups.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<CustomerGroup>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}