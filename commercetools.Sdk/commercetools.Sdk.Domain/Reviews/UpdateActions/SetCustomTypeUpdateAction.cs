namespace commercetools.Sdk.Domain.Reviews
{
    public class SetCustomTypeUpdateAction : UpdateAction<Review>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}