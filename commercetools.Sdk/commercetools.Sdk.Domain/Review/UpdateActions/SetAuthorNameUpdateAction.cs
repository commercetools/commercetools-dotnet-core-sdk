namespace commercetools.Sdk.Domain.Reviews
{
    public class SetAuthorNameUpdateAction : UpdateAction<Review>
    {
        public string Action => "setAuthorName";
        public string AuthorName { get; set; }
    }
}