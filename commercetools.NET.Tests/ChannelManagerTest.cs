using commercetools.Channels;
using commercetools.Channels.UpdateActions;
using commercetools.Common;
using commercetools.GeoLocation;
using commercetools.Project;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the ChannelManager class.
    /// </summary>
    [TestFixture]
    public class ChannelManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Channel> _testChannels;
        
        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            _testChannels = new List<Channel>();

            for (int i = 0; i < 5; i++)
            {
                ChannelDraft channelDraft = Helper.GetTestChannelDraft(_project);

                Task<Response<Channel>> channelTask = _client.Channels().CreateChannelAsync(channelDraft);
                channelTask.Wait();
                Assert.IsTrue(channelTask.Result.Success);

                Channel channel = channelTask.Result.Result;
                Assert.NotNull(channel.Id);

                _testChannels.Add(channel);
            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (Channel channel in _testChannels)
            {
                Task<Response<Channel>> channelTask = _client.Channels().DeleteChannelAsync(channel);
                channelTask.Wait();
            }
        }

        /// <summary>
        /// Tests the ChannelManager.GetChannelByIdAsync method.
        /// </summary>
        /// <see cref="ChannelManager.GetChannelByIdAsync"/>
        [Test]
        public async Task ShouldGetChannelByIdAsync()
        {
            Response<Channel> response = await _client.Channels().GetChannelByIdAsync(_testChannels[0].Id);
            Assert.IsTrue(response.Success);

            Channel channel = response.Result;
            Assert.NotNull(channel.Id);
            Assert.AreEqual(channel.Id, _testChannels[0].Id);
        }

        /// <summary>
        /// Tests the ChannelManager.QueryChannelsAsync method.
        /// </summary>
        /// <see cref="ChannelManager.QueryChannelsAsync"/>
        [Test]
        public async Task ShouldQueryChannelsAsync()
        {
            Response<ChannelQueryResult> response = await _client.Channels().QueryChannelAsync();
            Assert.IsTrue(response.Success);

            ChannelQueryResult channelQueryResult = response.Result;
            Assert.NotNull(channelQueryResult.Results);
            Assert.GreaterOrEqual(channelQueryResult.Results.Count, 1);

            int limit = 2;
            response = await _client.Channels().QueryChannelAsync(limit: limit);
            Assert.IsTrue(response.Success);

            channelQueryResult = response.Result;
            Assert.NotNull(channelQueryResult.Results);
            Assert.LessOrEqual(channelQueryResult.Results.Count, limit);
        }

        /// <summary>
        /// Tests the ChannelManager.CreateChannelAsync and ChannelManager.DeleteChannelAsync methods.
        /// </summary>
        /// <see cref="ChannelManager.CreateChannelAsync"/>
        /// <seealso cref="ChannelManager.DeleteChannelAsync(commercetools.Channels.Channel)"/>
        [Test]
        public async Task ShouldCreateAndDeleteChannelAsync()
        {
            ChannelDraft channelDraft = Helper.GetTestChannelDraft(_project);
            Response<Channel> response = await _client.Channels().CreateChannelAsync(channelDraft);
            Assert.IsTrue(response.Success);

            Channel channel = response.Result;
            Assert.NotNull(channel.Id);

            string deletedChannelId = channel.Id;

            response = await _client.Channels().DeleteChannelAsync(channel);
            Assert.IsTrue(response.Success);

            response = await _client.Channels().GetChannelByIdAsync(deletedChannelId);
            Assert.IsFalse(response.Success);
        }

        /// <summary>
        /// Tests the ChannelManager.ShouldUpdateChannelAndSetGeolocationAsPointAsync method.
        /// </summary>
        /// <see cref="ChannelManager.UpdateChannelAsync(commercetools.Channels.Channel, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdateChannelAndSetGeoLocationAsyncAsPoint()
        {
            Point geoLocation = new Point(Helper.GetRandomDouble(0,10), Helper.GetRandomDouble(0, 10));

            SetGeoLocationAction setGeoLocationAction = new SetGeoLocationAction(geoLocation);

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(setGeoLocationAction);

            Response<Channel> response = await _client.Channels().UpdateChannelAsync(_testChannels[2], actions);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(geoLocation.Type, ((Point)response.Result.GeoLocation).Type);
            Assert.AreEqual(geoLocation.Latitude, ((Point)response.Result.GeoLocation).Latitude);
            Assert.AreEqual(geoLocation.Longitude, ((Point)response.Result.GeoLocation).Longitude);
        }
    }
}