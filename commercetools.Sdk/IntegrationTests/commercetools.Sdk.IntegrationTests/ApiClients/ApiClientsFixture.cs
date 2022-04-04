using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.ApiClients;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.ApiClients
{
    public static class ApiClientsFixture
    {
        public const string ClientScope = "view_products";
        #region DraftBuilds
        public static ApiClientDraft DefaultApiClientDraft(ApiClientDraft apiClientDraft)
        {
            var random = TestingUtility.RandomInt();
            apiClientDraft.Name = $"ApiClient_{random}";
            apiClientDraft.Scope = ClientScope;
            return apiClientDraft;
        }
        #endregion

        #region WithApiClient

        public static async Task WithApiClient( IClient client, Func<ApiClientDraft, ApiClientDraft> draftAction, Action<ApiClient> func)
        {
            await With(client, new ApiClientDraft(), draftAction, func);
        }

        public static async Task WithApiClient( IClient client, Func<ApiClientDraft, ApiClientDraft> draftAction, Func<ApiClient, Task> func)
        {
            await WithAsync(client, new ApiClientDraft(), draftAction, func);
        }
        #endregion
        
    }
}
