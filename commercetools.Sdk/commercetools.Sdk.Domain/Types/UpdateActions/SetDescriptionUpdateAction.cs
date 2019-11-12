namespace commercetools.Sdk.Domain.Types.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<Type>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}