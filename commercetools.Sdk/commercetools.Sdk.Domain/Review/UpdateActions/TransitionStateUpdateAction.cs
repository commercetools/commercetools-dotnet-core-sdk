namespace commercetools.Sdk.Domain.Reviews
{
    public class TransitionStateUpdateAction : UpdateAction<Review>
    {
        public string Action => "transitionState";
        public ResourceIdentifier State { get; set; }
        public bool Force { get; set; }
    }
}