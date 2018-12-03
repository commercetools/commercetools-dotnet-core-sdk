namespace commercetools.Sdk.Client
{
    using commercetools.Sdk.Domain;

    // TODO Implement QueryParameters in request builder
    public abstract class Command<T>
    {
        public IQueryParameters<T> QueryParameters { get; set; }
        public abstract System.Type ResourceType { get; } 
    }
}