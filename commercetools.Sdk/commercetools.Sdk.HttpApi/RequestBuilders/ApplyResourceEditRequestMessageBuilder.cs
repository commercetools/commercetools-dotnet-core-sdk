using System;
using System.Collections.Generic;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.OrderEdits;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class ApplyResourceEditRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public ApplyResourceEditRequestMessageBuilder(
            ISerializerService serializerService,
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
            : base(endpointRetriever, parametersBuilderFactory)
        {
            this.serializerService = serializerService;
        }

        private static HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(ApplyResourceEditCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), this.GetHttpContent(command), HttpMethod);
        }

        private HttpContent GetHttpContent<T>(ApplyResourceEditCommand<T> command)
        {
            var requestBody = new
            {
                EditVersion = command.EditVersion,
                ResourceVersion = command.ResourceVersion
            };
            return new StringContent(this.serializerService.Serialize(requestBody));
        }

        private Uri GetRequestUri<T>(ApplyResourceEditCommand<T> command)
        {
            string requestUri = this.GetMessageBase<OrderEdit>();
            requestUri += $"/{command.Id}/apply";
            return new Uri(requestUri, UriKind.Relative);
        }
    }
}
