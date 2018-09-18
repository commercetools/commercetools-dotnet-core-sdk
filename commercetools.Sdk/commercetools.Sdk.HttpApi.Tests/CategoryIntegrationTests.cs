using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryIntegrationTests
    {
        [Fact]
        public void GetCategoryById()
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
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { getByIdRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IRequestBuilder requestBuilder = new RequestBuilder(requestMessageBuilderFactory);
            
            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            string categoryId = "2bafc816-4223-4ff0-ac8a-0f08a8f29fd6";
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand(new Guid(categoryId))).Result;
            Assert.Equal(categoryId, category.Id.ToString());
        }

        [Fact]
        public void GetCategoryByKey()
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
            GetByKeyRequestMessageBuilder getByKeyRequestMessageBuilder = new GetByKeyRequestMessageBuilder(clientConfiguration);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { getByKeyRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IRequestBuilder requestBuilder = new RequestBuilder(requestMessageBuilderFactory);
            
            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            string categoryKey = "c2";
            Category category = commerceToolsClient.Execute<Category>(new GetByKeyCommand(categoryKey)).Result;
            Assert.Equal(categoryKey, category.Key.ToString());
        }

        [Fact]
        public void CreateCategory()
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
            CreateRequestMessageBuilder createRequestMessageBuilder = new CreateRequestMessageBuilder(serializerService, clientConfiguration);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { createRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IRequestBuilder requestBuilder = new RequestBuilder(requestMessageBuilderFactory);

            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            CategoryDraft categoryDraft = new CategoryDraft();
            string categoryName = TestUtils.RandomString(4);
            LocalizedString localizedStringName = new LocalizedString();
            localizedStringName.Add("en", categoryName);
            categoryDraft.Name = localizedStringName;
            string slug = TestUtils.RandomString(5);
            LocalizedString localizedStringSlug = new LocalizedString();
            localizedStringSlug.Add("en", slug);
            categoryDraft.Slug = localizedStringSlug;
            Category category = commerceToolsClient.Execute<Category>(new CreateCommand(categoryDraft)).Result;
            Assert.Equal(categoryName, category.Name["en"]);
        }

        [Fact]
        public void UpdateCategoryById()
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
            UpdateByIdRequestMessageBuilder updateByIdRequestMessageBuilder = new UpdateByIdRequestMessageBuilder(serializerService, clientConfiguration);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { updateByIdRequestMessageBuilder, getByIdRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IRequestBuilder requestBuilder = new RequestBuilder(requestMessageBuilderFactory);

            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            string categoryId = "8994e5d7-d81f-4480-af60-286dc96c1fe8";
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand(new Guid(categoryId))).Result;
            string currentKey = category.Key;
            SetKey setKeyAction = new SetKey();
            setKeyAction.Key = "newKey" + TestUtils.RandomString(3);
            Category updatedCategory = commerceToolsClient.Execute<Category>(new UpdateByIdCommand(new Guid(category.Id), category.Version, new List<UpdateAction>() { setKeyAction })).Result;
            Assert.Equal(updatedCategory.Key, setKeyAction.Key);
        }

        [Fact]
        public void UpdateCategoryByKey()
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
            UpdateByKeyRequestMessageBuilder updateByKeyRequestMessageBuilder = new UpdateByKeyRequestMessageBuilder(serializerService, clientConfiguration);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { updateByKeyRequestMessageBuilder, getByIdRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IRequestBuilder requestBuilder = new RequestBuilder(requestMessageBuilderFactory);

            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            string categoryId = "8994e5d7-d81f-4480-af60-286dc96c1fe8";
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand(new Guid(categoryId))).Result;
            ChangeOrderHint changeOrderHint = new ChangeOrderHint();
            changeOrderHint.OrderHint = TestUtils.RandomString(6);
            SetExternalId setExternalId = new SetExternalId();
            setExternalId.ExternalId = TestUtils.RandomString(5);
            Category updatedCategory = commerceToolsClient.Execute<Category>(new UpdateByKeyCommand(category.Key, category.Version, new List<UpdateAction>() { changeOrderHint, setExternalId })).Result;
            Assert.Equal(updatedCategory.OrderHint, changeOrderHint.OrderHint);
            Assert.Equal(updatedCategory.ExternalId, setExternalId.ExternalId);
        }
    }
}
