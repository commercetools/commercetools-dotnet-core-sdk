using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class RequestBuilder : IRequestBuilder
    {
        private IClientConfiguration clientConfiguration;

        private IDictionary<Type, string> mapping = new Dictionary<Type, string>()
        {
            {  typeof(Category), "categories" }
        };

        public RequestBuilder(IClientConfiguration clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
        }

        public HttpRequestMessage GetRequestMessageById<T>(Guid guid)
        {
            return GetRequestMessage<T>($"/{guid}", HttpMethod.Get);
        }

        public HttpRequestMessage GetRequestMessageByKey<T>(string key)
        {
            return GetRequestMessage<T>($"/key={key}", HttpMethod.Get);
        }

        private HttpRequestMessage GetRequestMessage<T>(string endOfRequestUri, HttpMethod httpMethod)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string requestUri = this.GetMessageBase<T>() + endOfRequestUri;
            request.RequestUri = new Uri(requestUri);
            request.Method = httpMethod;
            return request;
        }

        public HttpRequestMessage GetRequestMessage<T>(IHttpApiCommand httpApiCommand)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string requestUri = this.GetMessageBase<T>() + httpApiCommand.RequestUriEnd;
            request.RequestUri = new Uri(requestUri);
            request.Method = httpApiCommand.HttpMethod;
            return request;
        }

        private string GetMessageBase<T>()
        {
            return this.clientConfiguration.ApiBaseAddress + $"{this.clientConfiguration.ProjectKey}/{this.mapping[typeof(T)]}";
        }
    }
}