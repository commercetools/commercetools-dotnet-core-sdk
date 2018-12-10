namespace commercetools.Sdk.Domain.Zones.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<Zone>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}