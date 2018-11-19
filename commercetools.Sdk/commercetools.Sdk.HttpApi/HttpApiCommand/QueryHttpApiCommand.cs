using commercetools.Sdk.Client;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class QueryHttpApiCommand<T> : IHttpApiCommand<QueryCommand<T>, T>
    {
        private readonly QueryRequestMessageBuilder requestBuilder;
        private QueryCommand<T> command;

        public QueryHttpApiCommand(QueryCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<QueryRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }
    }
}