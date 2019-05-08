using System;
using System.Collections.Generic;
using System.Net.Http;
using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi.AdditionalParameters;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public abstract class RequestMessageBuilderBase
    {
        private readonly IEndpointRetriever endpointRetriever;
        private readonly IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory;

        protected RequestMessageBuilderBase(
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
        {
            this.endpointRetriever = endpointRetriever;
            this.parametersBuilderFactory = parametersBuilderFactory;
        }

        protected string GetMessageBase<T>()
        {
            return $"{this.endpointRetriever.GetEndpoint<T>()}";
        }

        protected List<KeyValuePair<string, string>> GetAdditionalParameters(IAdditionalParameters additionalParameters)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (additionalParameters == null)
            {
                return queryStringParameters;
            }

            IAdditionalParametersBuilder additionalParametersBuilder = this.parametersBuilderFactory.GetParameterBuilder(additionalParameters);
            queryStringParameters.AddRange(additionalParametersBuilder.GetAdditionalParameters(additionalParameters));
            return queryStringParameters;
        }

        protected HttpRequestMessage GetRequestMessage<T>(Uri requestUri, HttpContent httpContent, HttpMethod httpMethod)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Method = httpMethod
            };
            if (httpContent != null)
            {
                request.Content = httpContent;
            }

            return request;
        }
    }
}
