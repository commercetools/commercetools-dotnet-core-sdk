namespace commercetools.Sdk.Client
{
    using commercetools.Sdk.Domain;

    // TODO Implement QueryParameters in request builder
    public abstract class Command<T>
    {
        IQueryParameters<T> QueryParameters { get; set; }
    }
}