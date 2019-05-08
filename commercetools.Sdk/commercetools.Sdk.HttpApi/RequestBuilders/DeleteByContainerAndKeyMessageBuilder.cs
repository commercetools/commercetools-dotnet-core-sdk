using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.CustomObject;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class DeleteByContainerAndKeyMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public DeleteByContainerAndKeyMessageBuilder(
            ISerializerService serializerService,
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
            : base(endpointRetriever, parametersBuilderFactory)
        {
            this.serializerService = serializerService;
        }

        private static HttpMethod HttpMethod => HttpMethod.Delete;

        public HttpRequestMessage GetRequestMessage<T>(DeleteByContainerAndKeyCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null, HttpMethod);
        }

        private Uri GetRequestUri<T>(DeleteByContainerAndKeyCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            requestUri += $"/{command.Container}/{command.Key}";
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.Expand != null)
            {
                queryStringParameters.AddRange(command.Expand.Select(x => new KeyValuePair<string, string>("expand", x)));
            }

            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));

            if (command.DataErasure != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("dataErasure", command.DataErasure.ToString()));
            }

            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri, UriKind.Relative);
        }
    }
}
