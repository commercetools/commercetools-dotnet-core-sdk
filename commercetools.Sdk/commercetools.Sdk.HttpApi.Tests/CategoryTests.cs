using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryTests
    {
        [Fact]
        public void GetCategoryById()
        {
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            IHttpClientFactory httpClientFactoryAuth = new MockHttpClientFactory(null);
            ITokenProvider clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(httpClientFactoryAuth, clientConfiguration, tokenStoreManager);
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            ITokenProviderFactory tokenProviderFactory = new TokenProviderFactory(new List<ITokenProvider>() { clientCredentialsTokenProvider });
            AuthorizationHandler authorizationHandler = new AuthorizationHandler(tokenProviderFactory, tokenFlowRegister);
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(authorizationHandler);
            IRequestBuilder requestBuilder = new RequestBuilder(clientConfiguration);
            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder);
            string categoryId = "f40fcd15-b1c2-4279-9cfa-f6083e6a2988";
            Category category = commerceToolsClient.GetByIdAsync<Category>(new Guid(categoryId)).Result;
            Assert.Equal(categoryId, category.Id.ToString());
        }

        [Fact]
        public void GetCategoryByKey()
        {
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            IHttpClientFactory httpClientFactoryAuth = new MockHttpClientFactory(null);
            ITokenProvider clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(httpClientFactoryAuth, clientConfiguration, tokenStoreManager);
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            ITokenProviderFactory tokenProviderFactory = new TokenProviderFactory(new List<ITokenProvider>() { clientCredentialsTokenProvider });
            AuthorizationHandler authorizationHandler = new AuthorizationHandler(tokenProviderFactory, tokenFlowRegister);
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(authorizationHandler);
            IRequestBuilder requestBuilder = new RequestBuilder(clientConfiguration);
            IClient commerceToolsClient = new Client(httpClientFactory, requestBuilder);
            string categoryKey = "c2";
            Category category = commerceToolsClient.GetByKeyAsync<Category>(categoryKey).Result;
            Assert.Equal(categoryKey, category.Key.ToString());
        }
    }
}
