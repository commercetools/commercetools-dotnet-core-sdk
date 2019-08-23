namespace commercetools.Sdk.Domain.APIExtensions.UpdateActions
{
    public class ChangeDestinationUpdateAction : UpdateAction<Extension>
    {
        public string Action => "changeDestination";
        public Destination Destination { get; set; }
    }
}
