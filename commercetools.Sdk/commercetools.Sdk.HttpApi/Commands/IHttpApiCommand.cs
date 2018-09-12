using commercetools.Sdk.Client;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public interface IHttpApiCommand : ICommand
    {
        HttpMethod HttpMethod { get; }

        // TODO Think of a better name
        string RequestUriEnd { get; }
    }
}