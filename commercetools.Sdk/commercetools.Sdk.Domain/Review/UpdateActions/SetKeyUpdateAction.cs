namespace commercetools.Sdk.Domain.Reviews
{
    public class SetKeyUpdateAction : UpdateAction<Review>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}