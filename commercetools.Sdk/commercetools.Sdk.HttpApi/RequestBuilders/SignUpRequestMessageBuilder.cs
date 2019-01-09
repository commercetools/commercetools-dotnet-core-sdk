using System;
using System.Collections.Generic;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class SignUpRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public SignUpRequestMessageBuilder(
            ISerializerService serializerService,
            IClientConfiguration clientConfiguration,
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
            : base(clientConfiguration, endpointRetriever, parametersBuilderFactory)
        {
            this.serializerService = serializerService;
        }

        private HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(SignUpCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), this.GetHttpContent<T>(command), this.HttpMethod);
        }

        private HttpContent GetHttpContent<T>(SignUpCommand<T> command)
        {
            return new StringContent(this.serializerService.Serialize(command.Entity));
        }

        private Uri GetRequestUri<T>(SignUpCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + "/";
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri);
        }
    }
}