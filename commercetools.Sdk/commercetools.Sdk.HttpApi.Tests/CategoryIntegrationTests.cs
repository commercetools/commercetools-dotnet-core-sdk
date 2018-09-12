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
            IRequestBuilder requestBuilder = new RequestBuilder(clientConfiguration);
            
            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            string categoryId = "2bafc816-4223-4ff0-ac8a-0f08a8f29fd6";
            //Category category = commerceToolsClient.GetByIdAsync<Category>(new Guid(categoryId)).Result;
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
            IRequestBuilder requestBuilder = new RequestBuilder(clientConfiguration);
            
            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder, serializerService);
            string categoryKey = "c2";
            //Category category = commerceToolsClient.GetByKeyAsync<Category>(categoryKey).Result;
            Category category = commerceToolsClient.Execute<Category>(new GetByKeyCommand(categoryKey)).Result;
            Assert.Equal(categoryKey, category.Key.ToString());
        }
    }
}
