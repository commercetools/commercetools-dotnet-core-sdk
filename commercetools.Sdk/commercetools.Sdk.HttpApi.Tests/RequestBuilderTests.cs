using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class RequestBuilderTests
    {
        [Fact]
        public void GetRequestMessageBuilderFromFactory()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            GetRequestMessageBuilder getByIdRequestMessageBuilder = new GetRequestMessageBuilder(clientConfiguration);
            CreateRequestMessageBuilder createRequestMessageBuilder = new CreateRequestMessageBuilder(serializerService, clientConfiguration);
            UpdateRequestMessageBuilder updateByIdRequestMessageBuilder = new UpdateRequestMessageBuilder(serializerService, clientConfiguration);
            DeleteRequestMessageBuilder deleteByIdRequestMessageBuilder = new DeleteRequestMessageBuilder(clientConfiguration);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { getByIdRequestMessageBuilder, createRequestMessageBuilder, updateByIdRequestMessageBuilder, deleteByIdRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IRequestMessageBuilder requestMessageBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetRequestMessageBuilder>();
            Assert.Equal(typeof(GetRequestMessageBuilder), requestMessageBuilder.GetType());
        }

        [Fact]
        public void GetHttpApiCommand()
        {
            CreateCommand<Category> createCommand = new CreateCommand<Category>(new CategoryDraft());
            IEnumerable<Type> registeredTypes = new List<Type>() { typeof(CreateHttpApiCommand<>) };
            ISerializerService serializerService = TestUtils.GetSerializerService();
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            CreateRequestMessageBuilder createRequestMessageBuilder = new CreateRequestMessageBuilder(serializerService, clientConfiguration);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { createRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IHttpApiCommandFactory httpApiCommandFactory = new HttpApiCommandFactory(registeredTypes, requestMessageBuilderFactory);
            IHttpApiCommand httpApiCommand = httpApiCommandFactory.Create(createCommand);
            Assert.Equal(typeof(CreateHttpApiCommand<Category>), httpApiCommand.GetType());
        }
    }
}
