using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using commercetools.Sdk.HttpApi.AdditionalParameters;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class UpdateRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public UpdateRequestMessageBuilder(
            ISerializerService serializerService,
            IClientConfiguration clientConfiguration, 
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory) 
            : base(clientConfiguration, endpointRetriever, parametersBuilderFactory)
        {
            this.serializerService = serializerService;
        }

        private HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(UpdateCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), this.GetHttpContent<T>(command), this.HttpMethod);
        }

        private HttpContent GetHttpContent<T>(UpdateCommand<T> command)
        {
            var requestBody = new
            {
                Version = command.Version,
                Actions = command.UpdateActions
            };
            return new StringContent(this.serializerService.Serialize(requestBody));
        }

        private Uri GetRequestUri<T>(UpdateCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            if (command.ParameterKey == Parameters.Id)
            {
                requestUri += $"/{command.ParameterValue}";
            }
            else
            {
                requestUri += $"/{command.ParameterKey}={command.ParameterValue}";
            }

            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri);
        }
    }
}