namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Linq;
    using Microsoft.AspNetCore.WebUtilities;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    public class GetRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly IExpansionExpressionVisitor expansionExpressionVisitor;

        public GetRequestMessageBuilder(IClientConfiguration clientConfiguration, IExpansionExpressionVisitor expansionExpressionVisitor) : base(clientConfiguration)
        {
            this.expansionExpressionVisitor = expansionExpressionVisitor;
        }

        private HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(GetCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri(command), null, this.HttpMethod);
        }

        private Uri GetRequestUri<T>(GetCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            if (command.ParameterKey == Parameters.ID)
            {
                requestUri += $"/{command.ParameterValue}";
            }
            else
            {
                requestUri += $"/{command.ParameterKey}={command.ParameterValue}";
            }

            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.Expand != null)
            {
                queryStringParameters.AddRange(command.Expand.GetQueryStringParameters(this.expansionExpressionVisitor));
            }

            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri);
        }
    }
}