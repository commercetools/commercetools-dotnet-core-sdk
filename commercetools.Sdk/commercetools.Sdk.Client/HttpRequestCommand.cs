using System.Net.Http;

namespace commercetools.Sdk.Client
{
    public class HttpRequestCommand<T> : Command<T>
    {
        public HttpRequestCommand(HttpRequestMessage requestMessage)
        {
            RequestMessage = requestMessage;
        }

        public HttpRequestMessage RequestMessage { get; }

        public override System.Type ResourceType => typeof(T);
    }
}