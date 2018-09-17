using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class RequestBuilder : IRequestBuilder
    {
        private IDictionary<Type, string> mapping = new Dictionary<Type, string>()
        {
            {  typeof(Category), "categories" }
        };

        private IClientConfiguration clientConfiguration;
        private IRequestMessageBuilderFactory requestMessageFactory;

        public RequestBuilder(IRequestMessageBuilderFactory requestMessageFactory, IClientConfiguration clientConfiguration)
        {
            this.requestMessageFactory = requestMessageFactory;
            this.clientConfiguration = clientConfiguration;
        }

        private string GetMessageBase<T>()
        {
            return this.clientConfiguration.ApiBaseAddress + $"{this.clientConfiguration.ProjectKey}/{this.mapping[typeof(T)]}";
        }

        private HttpRequestMessage GetRequestMessage<T>(IRequestMessageBuilder requestMessageBuilder)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string requestUri = this.GetMessageBase<T>() + requestMessageBuilder.RequestUriEnd;
            request.RequestUri = new Uri(requestUri);
            request.Method = requestMessageBuilder.HttpMethod;
            return request;
        }

        public HttpRequestMessage GetRequestMessage<T>(ICommand command)
        {
            IRequestMessageBuilder requestMessageBuilder = this.requestMessageFactory.GetRequestMessageBuilder(command);
            return this.GetRequestMessage<T>(requestMessageBuilder);      
        }        
    }
}