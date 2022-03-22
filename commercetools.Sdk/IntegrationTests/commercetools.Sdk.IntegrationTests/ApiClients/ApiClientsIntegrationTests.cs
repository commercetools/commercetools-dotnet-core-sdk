using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.ApiClients;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.ApiClients.ApiClientsFixture;

namespace commercetools.Sdk.IntegrationTests.ApiClients
{
    [Collection("Integration Tests")]
    public class ApiClientsIntegrationTests
    {
        private readonly IClient client;
        const string Skip = "Disable because of Insufficient scope";

        public ApiClientsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact(Skip = Skip)]
        public async Task CreateApiClient()
        {
            await WithApiClient(
                client, DefaultApiClientDraft,
                async apiClient =>
                {
                    Assert.NotNull(apiClient);
                    Assert.Equal(ClientScope, apiClient.Scope);
                });
        }

        [Fact(Skip = Skip)]
        public async Task GetApiClientById()
        {
            await WithApiClient(
                client, DefaultApiClientDraft,
                async apiClient =>
                {
                    var retrievedApiClient = await client
                        .ExecuteAsync(new GetByIdCommand<ApiClient>(apiClient.Id));
                    Assert.NotNull(retrievedApiClient);
                });
        }
        
        [Fact(Skip = Skip)]
        public async Task QueryApiClients()
        {
            var key = $"QueryApiClients-{TestingUtility.RandomString()}";
            await WithApiClient(
                client, DefaultApiClientDraft,
                async apiClient =>
                {
                    var queryCommand = new QueryCommand<ApiClient>();
                    queryCommand.Where(p => p.Id == apiClient.Id.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                });
        }

        [Fact(Skip = Skip)]
        public async Task DeleteApiClientById()
        {
            var key = $"DeleteApiClientById-{TestingUtility.RandomString()}";
            await WithApiClient(
                client, DefaultApiClientDraft,
                async apiClient =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<ApiClient>(apiClient));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<ApiClient>(apiClient))
                    );
                });
        }
    }
}