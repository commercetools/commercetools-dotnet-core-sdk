using System.Net.Http;
using commercetools.Sdk.Client;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class HttpRequestHttpApiCommand<T> : IHttpApiCommandGeneric<HttpRequestCommand<T>, T>
    {
        private readonly HttpRequestCommand<T> requestCommand;

        public HttpRequestHttpApiCommand(HttpRequestCommand<T> requestCommand)
        {
            this.requestCommand = requestCommand;
        }

        public HttpRequestMessage HttpRequestMessage => requestCommand.RequestMessage;
    }
}