using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Channels;
using Type = commercetools.Sdk.Domain.Type;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.Channels
{
    public static class ChannelsFixture
    {
        #region DraftBuilds
        public static ChannelDraft DefaultChannelDraft(ChannelDraft channelDraft)
        {
            var random = TestingUtility.RandomInt();
            channelDraft.Key = $"Key_{random}";
            channelDraft.Name = new LocalizedString() {{"en", $"Channel_{random}"}};
            channelDraft.Roles = new List<ChannelRole>
            {
                ChannelRole.InventorySupply
            };

            return channelDraft;
        }
        public static ChannelDraft DefaultChannelDraftWithKey(ChannelDraft draft, string key)
        {
            var channelDraft = DefaultChannelDraft(draft);
            channelDraft.Key = key;
            return channelDraft;
        }
        public static ChannelDraft DefaultChannelDraftWithRoles(ChannelDraft draft, List<ChannelRole> roles)
        {
            var channelDraft = DefaultChannelDraft(draft);
            channelDraft.Roles = roles;
            return channelDraft;
        }

        public static ChannelDraft DefaultChannelDraftWithCustomType(ChannelDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };

            var channelDraft = DefaultChannelDraft(draft);
            channelDraft.Custom = customFieldsDraft;

            return channelDraft;
        }
        #endregion

        #region WithChannel

        public static async Task WithChannel( IClient client, Action<Channel> func)
        {
            await With(client, new ChannelDraft(), DefaultChannelDraft, func);
        }
        public static async Task WithChannel( IClient client, Func<ChannelDraft, ChannelDraft> draftAction, Action<Channel> func)
        {
            await With(client, new ChannelDraft(), draftAction, func);
        }

        public static async Task WithChannel( IClient client, Func<Channel, Task> func)
        {
            await WithAsync(client, new ChannelDraft(), DefaultChannelDraft, func);
        }
        public static async Task WithChannel( IClient client, Func<ChannelDraft, ChannelDraft> draftAction, Func<Channel, Task> func)
        {
            await WithAsync(client, new ChannelDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableChannel

        public static async Task WithUpdateableChannel(IClient client, Func<Channel, Channel> func)
        {
            await WithUpdateable(client, new ChannelDraft(), DefaultChannelDraft, func);
        }

        public static async Task WithUpdateableChannel(IClient client, Func<ChannelDraft, ChannelDraft> draftAction, Func<Channel, Channel> func)
        {
            await WithUpdateable(client, new ChannelDraft(), draftAction, func);
        }

        public static async Task WithUpdateableChannel(IClient client, Func<Channel, Task<Channel>> func)
        {
            await WithUpdateableAsync(client, new ChannelDraft(), DefaultChannelDraft, func);
        }
        public static async Task WithUpdateableChannel(IClient client, Func<ChannelDraft, ChannelDraft> draftAction, Func<Channel, Task<Channel>> func)
        {
            await WithUpdateableAsync(client, new ChannelDraft(), draftAction, func);
        }

        #endregion
    }
}
