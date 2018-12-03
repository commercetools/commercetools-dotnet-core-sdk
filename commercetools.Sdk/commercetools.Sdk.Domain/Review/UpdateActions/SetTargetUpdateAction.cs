namespace commercetools.Sdk.Domain.Reviews
{
    public class SetTargetUpdateAction : UpdateAction<Review>
    {
        public string Action => "setTarget";
        public ResourceIdentifier Target { get; set; }
    }
}