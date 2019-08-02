namespace commercetools.Sdk.Domain.Stores.UpdateActions
{
    public class SetNameUpdateAction : UpdateAction<Store>
    {
        public string Action => "setName";
        public LocalizedString Name { get; set; }
    }
}
