namespace commercetools.Sdk.Domain.TaxRates
{
    public class SetDescriptionUpdateAction : UpdateAction<TaxRate>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}