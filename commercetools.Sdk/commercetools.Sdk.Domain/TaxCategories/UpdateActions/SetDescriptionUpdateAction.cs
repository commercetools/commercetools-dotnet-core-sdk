namespace commercetools.Sdk.Domain.TaxCategories.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<TaxCategory>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}