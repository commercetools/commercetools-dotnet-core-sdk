using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Channels;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Channels
{
    public class ChannelFixture : ClientFixture, IDisposable
    {
        public List<Channel> ChannelsToDelete { get; }

        public ChannelFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.ChannelsToDelete = new List<Channel>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ChannelsToDelete.Reverse();
            foreach (Channel channel in this.ChannelsToDelete)
            {
                var deletedType = this.TryDeleteResource(channel).Result;
            }
        }
        public ChannelDraft GetChannelDraft(ChannelRole role)
        {
            ChannelDraft channelDraft = new ChannelDraft
            {
                Key = TestingUtility.RandomString(10),
                Roles = new List<ChannelRole> {role}
            };
            return channelDraft;
        }

        public Channel CreateChannel(ChannelRole role = ChannelRole.Primary)
        {
            return this.CreateChannel(this.GetChannelDraft(role));
        }

        public Channel CreateChannel(ChannelDraft channelDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Channel channel = commerceToolsClient.ExecuteAsync(new CreateCommand<Channel>(channelDraft)).Result;
            return channel;
        }
    }
}
