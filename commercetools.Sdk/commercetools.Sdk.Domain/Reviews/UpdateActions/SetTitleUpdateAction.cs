namespace commercetools.Sdk.Domain.Reviews
{
    public class SetTitleUpdateAction : UpdateAction<Review>
    {
        public string Action => "setTitle";
        public string Title { get; set; }
    }
}