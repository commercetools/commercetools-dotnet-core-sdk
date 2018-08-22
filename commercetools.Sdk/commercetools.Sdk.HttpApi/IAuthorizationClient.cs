namespace commercetools.Sdk.HttpApi
{
    using System.Net.Http;

    public interface IAuthorizationClient
    {
        HttpClient Client { get; }
    }
}