using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class UploadImageRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public UploadImageRequestMessageBuilder(
            IClientConfiguration clientConfiguration,
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
            : base(clientConfiguration, endpointRetriever, parametersBuilderFactory)
        {
        }

        private HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(UploadImageCommand<T> command)
        {
            return null;
        }
    }
}