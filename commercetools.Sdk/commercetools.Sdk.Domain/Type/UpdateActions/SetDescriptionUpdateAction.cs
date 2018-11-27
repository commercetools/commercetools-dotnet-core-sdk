namespace commercetools.Sdk.Domain
{
    public class SetDescriptionUpdateAction : UpdateAction<Type>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}