using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryTests
    {
        [Fact]
        public void GetCategoryById()
        {
            ISerializerService serializerService = new SerializerService(SettingsFactory.Create);
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            IHttpClientFactory httpClientFactoryAuth = new MockHttpClientFactory(null);
            ITokenProvider clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(httpClientFactoryAuth, clientConfiguration, tokenStoreManager, serializerService);
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            ITokenProviderFactory tokenProviderFactory = new TokenProviderFactory(new List<ITokenProvider>() { clientCredentialsTokenProvider });
            AuthorizationHandler authorizationHandler = new AuthorizationHandler(tokenProviderFactory, tokenFlowRegister);
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(authorizationHandler);
            IRequestBuilder requestBuilder = new RequestBuilder(clientConfiguration);

            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            string categoryId = "f40fcd15-b1c2-4279-9cfa-f6083e6a2988";
            //Category category = commerceToolsClient.GetByIdAsync<Category>(new Guid(categoryId)).Result;
            Category category = commerceToolsClient.Execute<Category>(new GetByIdCommand(new Guid(categoryId))).Result;
            Assert.Equal(categoryId, category.Id.ToString());
        }

        [Fact]
        public void GetCategoryByKey()
        {
            ISerializerService serializerService = new SerializerService(SettingsFactory.Create);
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            IHttpClientFactory httpClientFactoryAuth = new MockHttpClientFactory(null);
            ITokenProvider clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(httpClientFactoryAuth, clientConfiguration, tokenStoreManager, serializerService);
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            ITokenProviderFactory tokenProviderFactory = new TokenProviderFactory(new List<ITokenProvider>() { clientCredentialsTokenProvider });
            AuthorizationHandler authorizationHandler = new AuthorizationHandler(tokenProviderFactory, tokenFlowRegister);
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(authorizationHandler);
            IRequestBuilder requestBuilder = new RequestBuilder(clientConfiguration);

            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            string categoryKey = "c2";
            //Category category = commerceToolsClient.GetByKeyAsync<Category>(categoryKey).Result;
            Category category = commerceToolsClient.Execute<Category>(new GetByKeyCommand(categoryKey)).Result;
            Assert.Equal(categoryKey, category.Key.ToString());
        }
    }
}