namespace commercetools.Sdk.Domain.Reviews
{
    public class SetTextUpdateAction : UpdateAction<Review>
    {
        public string Action => "setText";
        public string Text { get; set; }
    }
}