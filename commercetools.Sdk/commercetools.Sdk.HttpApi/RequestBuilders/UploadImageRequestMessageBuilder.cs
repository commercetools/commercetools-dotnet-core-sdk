using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.UploadImageParameters;
using Microsoft.AspNetCore.WebUtilities;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class UploadImageRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly IParametersBuilderFactory<IUploadImageParametersBuilder> uploadParametersBuilderFactory;

        public UploadImageRequestMessageBuilder(
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory,
            IParametersBuilderFactory<IUploadImageParametersBuilder> uploadParametersBuilderFactory)
            : base(endpointRetriever, parametersBuilderFactory)
        {
            this.uploadParametersBuilderFactory = uploadParametersBuilderFactory;
        }

        private static HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(UploadImageCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), this.GetHttpContent<T>(command), HttpMethod);
        }

        private HttpContent GetHttpContent<T>(UploadImageCommand<T> command)
        {
            HttpContent content = new StreamContent(command.Image);
            content.Headers.ContentLength = command.Image.Length;
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(command.ContentType);
            return content;
        }

        private Uri GetRequestUri<T>(UploadImageCommand<T> command)
        {
            string requestUri = $"{this.GetMessageBase<T>()}/{command.Id}/images";
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            if (command.Parameters != null)
            {
                IUploadImageParametersBuilder parametersBuilder = this.uploadParametersBuilderFactory.GetParameterBuilder(command.Parameters);
                queryStringParameters.AddRange(parametersBuilder.GetUploadImageParameters(command.Parameters));
            }

            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri, UriKind.Relative);
        }
    }
}
