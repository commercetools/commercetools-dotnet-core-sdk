using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public interface IHttpApiCommand
    {
        HttpRequestMessage HttpRequestMessage { get; }
    }

    public interface IHttpApiCommand<TU, T> : IHttpApiCommand
    {
      
    }
}