using System;
using System.Collections.Generic;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.SearchParameters;
using Microsoft.AspNetCore.WebUtilities;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    /// <summary>
    /// This class builds request messages for <see cref="commercetools.Sdk.Client.CheckCommand{T}"/>
    /// </summary>
    /// <seealso cref="commercetools.Sdk.HttpApi.RequestBuilders.RequestMessageBuilderBase" />
    /// <seealso cref="commercetools.Sdk.HttpApi.RequestBuilders.IRequestMessageBuilder" />
    public class CheckRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRequestMessageBuilder"/> class.
        /// </summary>
        /// <param name="endpointRetriever">The endpoint retriever.</param>
        /// <param name="parametersBuilderFactory">The parameters builder factory.</param>
        public CheckRequestMessageBuilder(
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
            : base(endpointRetriever, parametersBuilderFactory)
        {
        }

        private static HttpMethod HttpMethod => HttpMethod.Head;

        /// <summary>
        /// Gets the request message.
        /// </summary>
        /// <typeparam name="T">The domain specific type.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>The request message.</returns>
        public HttpRequestMessage GetRequestMessage<T>(CheckCommand<T> command)
            where T : Resource<T>, ICheckable<T>
        {
            return this.GetRequestMessage<T>(this.GetRequestUri(command), null, HttpMethod);
        }

        private Uri GetRequestUri<T>(CheckCommand<T> command)
            where T : Resource<T>, ICheckable<T>
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

            var queryStringParameters = new List<KeyValuePair<string, string>>();

            if (command is CheckByQueryCommand<T> checkByQueryCommand && checkByQueryCommand.Where.Count > 0)
            {
                foreach (var filter in checkByQueryCommand.Where)
                {
                    queryStringParameters.Add(new KeyValuePair<string, string>("where", filter));
                }
            }

            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri, UriKind.Relative);
        }
    }
}
