﻿namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Serialization;
    using System;
    using System.Net.Http;

    public class CreateRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public CreateRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
            this.serializerService = serializerService;
        }

        private HttpContent GetHttpContent<T>(CreateCommand<T> command)
        {
            return new StringContent(this.serializerService.Serialize(command.Entity));
        }

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(CreateCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), this.GetHttpContent<T>(command));
        }

        private Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/";
            return new Uri(requestUri);
        }
    }
}