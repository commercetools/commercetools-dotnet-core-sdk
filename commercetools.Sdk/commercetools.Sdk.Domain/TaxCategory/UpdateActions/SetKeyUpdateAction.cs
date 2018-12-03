namespace commercetools.Sdk.Domain.TaxRates
{
    public class SetKeyUpdateAction : UpdateAction<TaxRate>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}