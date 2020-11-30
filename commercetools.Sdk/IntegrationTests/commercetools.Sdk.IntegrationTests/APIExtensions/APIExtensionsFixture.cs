using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.APIExtensions;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.APIExtensions
{
    public static class ApiExtensionsFixture
    {
        #region DraftBuilds
        public static ExtensionDraft DefaultExtensionDraft(ExtensionDraft draft)
        {
            var random = TestingUtility.RandomInt();
            draft.Key = $"Key_{random}";
            draft.Triggers = GetTriggers(ExtensionResourceType.Customer);
            draft.Destination = GetHttpDestinationUsingAuthorizationHeader();
            return draft;
        }

        public static ExtensionDraft DefaultExtensionDraftForSpecificResourceType(ExtensionDraft draft, ExtensionResourceType resourceType)
        {
            var random = TestingUtility.RandomInt();
            draft.Key = $"Key_{random}";
            draft.Triggers = GetTriggers(resourceType);
            draft.Destination = GetHttpDestinationUsingAuthorizationHeader();
            return draft;
        }

        public static List<Trigger> GetTriggers(ExtensionResourceType resourceType)
        {
            var trigger = new Trigger
            {
                Actions = new List<TriggerType> {TriggerType.Create, TriggerType.Update},
                ResourceTypeId = resourceType
            };
            var triggers = new List<Trigger> { trigger };
            return triggers;
        }

        public static Destination GetHttpDestinationUsingAuthorizationHeader()
        {
            var destination = new HttpDestination
            {
                Url = "https://1i4axkp5vh.execute-api.eu-west-1.amazonaws.com/dev"
            };
            return destination;
        }
        #endregion

        #region WithExtension

        public static async Task WithExtension( IClient client, Action<Extension> func)
        {
            await With(client, new ExtensionDraft(), DefaultExtensionDraft, func);
        }
        public static async Task WithExtension( IClient client, Func<ExtensionDraft, ExtensionDraft> draftAction, Action<Extension> func)
        {
            await With(client, new ExtensionDraft(), draftAction, func);
        }

        public static async Task WithExtension( IClient client, Func<Extension, Task> func)
        {
            await WithAsync(client, new ExtensionDraft(), DefaultExtensionDraft, func);
        }
        public static async Task WithExtension( IClient client, Func<ExtensionDraft, ExtensionDraft> draftAction, Func<Extension, Task> func)
        {
            await WithAsync(client, new ExtensionDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableExtension

        public static async Task WithUpdateableExtension(IClient client, Func<Extension, Extension> func)
        {
            await WithUpdateable(client, new ExtensionDraft(), DefaultExtensionDraft, func);
        }

        public static async Task WithUpdateableExtension(IClient client, Func<ExtensionDraft, ExtensionDraft> draftAction, Func<Extension, Extension> func)
        {
            await WithUpdateable(client, new ExtensionDraft(), draftAction, func);
        }

        public static async Task WithUpdateableExtension(IClient client, Func<Extension, Task<Extension>> func)
        {
            await WithUpdateableAsync(client, new ExtensionDraft(), DefaultExtensionDraft, func);
        }
        public static async Task WithUpdateableExtension(IClient client, Func<ExtensionDraft, ExtensionDraft> draftAction, Func<Extension, Task<Extension>> func)
        {
            await WithUpdateableAsync(client, new ExtensionDraft(), draftAction, func);
        }

        #endregion
    }
}
