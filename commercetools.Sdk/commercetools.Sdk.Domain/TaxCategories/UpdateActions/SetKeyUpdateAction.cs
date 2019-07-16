namespace commercetools.Sdk.Domain.TaxCategories.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<TaxCategory>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}
