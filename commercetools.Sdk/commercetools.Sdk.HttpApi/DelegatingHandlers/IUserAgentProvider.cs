namespace commercetools.Sdk.HttpApi.DelegatingHandlers
{
    public interface IUserAgentProvider
    {
        string UserAgent { get; set; }
    }
}