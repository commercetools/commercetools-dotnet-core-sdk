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
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            GetByIdRequestMessageBuilder getByIdRequestMessageBuilder = new GetByIdRequestMessageBuilder(clientConfiguration);
            GetByKeyRequestMessageBuilder getByKeyRequestMessageBuilder = new GetByKeyRequestMessageBuilder(clientConfiguration);
            CreateRequestMessageBuilder createRequestMessageBuilder = new CreateRequestMessageBuilder(serializerService, clientConfiguration);
            UpdateByIdRequestMessageBuilder updateByIdRequestMessageBuilder = new UpdateByIdRequestMessageBuilder(serializerService, clientConfiguration);
            UpdateByKeyRequestMessageBuilder updateByKeyRequestMessageBuilder = new UpdateByKeyRequestMessageBuilder(serializerService, clientConfiguration);
            DeleteByIdRequestMessageBuilder deleteByIdRequestMessageBuilder = new DeleteByIdRequestMessageBuilder(clientConfiguration);
            DeleteByKeyRequestMessageBuilder deleteByKeyRequestMessageBuilder = new DeleteByKeyRequestMessageBuilder(clientConfiguration);
            IDictionary<Type, Type> mapping = new Dictionary<Type, Type>();
            mapping.Add(typeof(GetByIdCommand<>), typeof(GetByIdRequestMessageBuilder));
            mapping.Add(typeof(GetByKeyCommand<>), typeof(GetByKeyRequestMessageBuilder));
            mapping.Add(typeof(UpdateByIdCommand<>), typeof(UpdateByIdRequestMessageBuilder));
            mapping.Add(typeof(UpdateByKeyCommand<>), typeof(UpdateByKeyRequestMessageBuilder));
            mapping.Add(typeof(DeleteByIdCommand<>), typeof(DeleteByIdRequestMessageBuilder));
            mapping.Add(typeof(DeleteByKeyCommand<>), typeof(DeleteByKeyRequestMessageBuilder));
            mapping.Add(typeof(CreateCommand<>), typeof(CreateRequestMessageBuilder));
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { getByIdRequestMessageBuilder, getByKeyRequestMessageBuilder, createRequestMessageBuilder, updateByIdRequestMessageBuilder, updateByKeyRequestMessageBuilder, deleteByKeyRequestMessageBuilder, deleteByIdRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(mapping, requestMessageBuilders);
            IRequestMessageBuilder requestMessageBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder(new GetByIdCommand<Category>(new Guid()));
            Assert.Equal(typeof(GetByIdRequestMessageBuilder), requestMessageBuilder.GetType());
        }
    }
}
