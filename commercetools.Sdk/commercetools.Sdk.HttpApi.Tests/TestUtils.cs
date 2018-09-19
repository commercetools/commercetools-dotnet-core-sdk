using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class TestUtils
    {
        public static ClientConfiguration GetClientConfiguration(string clientSettings)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            return config.GetSection(clientSettings).Get<ClientConfiguration>();
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static IClient SetupClient()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            IHttpClientFactory httpClientFactoryAuth = new MockHttpClientFactory(null);
            ITokenProvider clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(httpClientFactoryAuth, clientConfiguration, tokenStoreManager, serializerService);
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            ITokenProviderFactory tokenProviderFactory = new TokenProviderFactory(new List<ITokenProvider>() { clientCredentialsTokenProvider });
            AuthorizationHandler authorizationHandler = new AuthorizationHandler(tokenProviderFactory, tokenFlowRegister);
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(authorizationHandler);
            GetByIdRequestMessageBuilder getByIdRequestMessageBuilder = new GetByIdRequestMessageBuilder(clientConfiguration);
            GetByKeyRequestMessageBuilder getByKeyRequestMessageBuilder = new GetByKeyRequestMessageBuilder(clientConfiguration);
            CreateRequestMessageBuilder createRequestMessageBuilder = new CreateRequestMessageBuilder(serializerService, clientConfiguration);
            UpdateByIdRequestMessageBuilder updateByIdRequestMessageBuilder = new UpdateByIdRequestMessageBuilder(serializerService, clientConfiguration);
            UpdateByKeyRequestMessageBuilder updateByKeyRequestMessageBuilder = new UpdateByKeyRequestMessageBuilder(serializerService, clientConfiguration);
            DeleteByIdRequestMessageBuilder deleteByIdRequestMessageBuilder = new DeleteByIdRequestMessageBuilder(clientConfiguration);
            DeleteByKeyRequestMessageBuilder deleteByKeyRequestMessageBuilder = new DeleteByKeyRequestMessageBuilder(clientConfiguration);
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(serializerService, clientConfiguration);
            IDictionary<Type, Type> mapping = new Dictionary<Type, Type>();
            mapping.Add(typeof(GetByIdCommand<>), typeof(GetByIdRequestMessageBuilder));
            mapping.Add(typeof(GetByKeyCommand<>), typeof(GetByKeyRequestMessageBuilder));
            mapping.Add(typeof(UpdateByIdCommand<>), typeof(UpdateByIdRequestMessageBuilder));
            mapping.Add(typeof(UpdateByKeyCommand<>), typeof(UpdateByKeyRequestMessageBuilder));
            mapping.Add(typeof(DeleteByIdCommand<>), typeof(DeleteByIdRequestMessageBuilder));
            mapping.Add(typeof(DeleteByKeyCommand<>), typeof(DeleteByKeyRequestMessageBuilder));
            mapping.Add(typeof(CreateCommand<>), typeof(CreateRequestMessageBuilder));
            mapping.Add(typeof(QueryCommand<>), typeof(QueryRequestMessageBuilder));
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { getByIdRequestMessageBuilder, getByKeyRequestMessageBuilder, createRequestMessageBuilder, updateByIdRequestMessageBuilder, updateByKeyRequestMessageBuilder, deleteByKeyRequestMessageBuilder, deleteByIdRequestMessageBuilder, queryRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(mapping, requestMessageBuilders);
            IRequestBuilder requestBuilder = new RequestBuilder(requestMessageBuilderFactory);

            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            return commerceToolsClient;
        }
    }
}