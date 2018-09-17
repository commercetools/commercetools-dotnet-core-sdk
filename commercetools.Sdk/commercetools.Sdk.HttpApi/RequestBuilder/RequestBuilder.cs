using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class RequestBuilder : IRequestBuilder
    {
        // TODO See if this should be moved to a different class
        private IDictionary<Type, string> mapping = new Dictionary<Type, string>()
        {
            {  typeof(Category), "categories" }
        };

        private IClientConfiguration clientConfiguration;
        private IRequestMessageBuilderFactory requestMessageBuilderFactory;
        private ISerializerService serializerService;

        public RequestBuilder(IRequestMessageBuilderFactory requestMessageBuilderFactory, IClientConfiguration clientConfiguration, ISerializerService serializerService)
        {
            this.requestMessageBuilderFactory = requestMessageBuilderFactory;
            this.clientConfiguration = clientConfiguration;
            this.serializerService = serializerService;
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
            if (requestMessageBuilder.RequestBody != null)
            {  
                request.Content = new StringContent(this.serializerService.Serialize(requestMessageBuilder.RequestBody));
            }
            return request;
        }

        public HttpRequestMessage GetRequestMessage<T>(ICommand command)
        {
            IRequestMessageBuilder requestMessageBuilder = this.requestMessageBuilderFactory.GetRequestMessageBuilder(command);
            return this.GetRequestMessage<T>(requestMessageBuilder);      
        }        
    }
}