using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using Microsoft.AspNetCore.WebUtilities;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    /// <summary>
    /// This class builds request messages for <see cref="commercetools.Sdk.Client.GetCommand{T}"/>
    /// </summary>
    /// <seealso cref="commercetools.Sdk.HttpApi.RequestBuilders.RequestMessageBuilderBase" />
    /// <seealso cref="commercetools.Sdk.HttpApi.RequestBuilders.IRequestMessageBuilder" />
    public class GetRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequestMessageBuilder"/> class.
        /// </summary>
        /// <param name="clientConfiguration">The client configuration.</param>
        /// <param name="endpointRetriever">The endpoint retriever.</param>
        /// <param name="parametersBuilderFactory">The parameters builder factory.</param>
        public GetRequestMessageBuilder(
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
            : base(endpointRetriever, parametersBuilderFactory)
        {
        }

        private static HttpMethod HttpMethod => HttpMethod.Get;

        /// <summary>
        /// Gets the request message.
        /// </summary>
        /// <typeparam name="T">The domain specific type.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>The request message.</returns>
        public HttpRequestMessage GetRequestMessage<T>(GetCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri(command), null, HttpMethod);
        }

        private Uri GetRequestUri<T>(GetCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            if (command.ParameterKey == Parameters.Id)
            {
                requestUri += $"/{command.ParameterValue}";
            }
            else if (!string.IsNullOrEmpty(command.ParameterKey) && command.ParameterValue != null)
            {
                requestUri += $"/{command.ParameterKey}={command.ParameterValue}";
            }

            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.Expand != null)
            {
                queryStringParameters.AddRange(command.Expand.Select(x => new KeyValuePair<string, string>("expand", x)));
            }

            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri, UriKind.Relative);
        }
    }
}
