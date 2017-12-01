using commercetools.Common;
using commercetools.Project;
using commercetools.Subscriptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the CustomerManager class.
    /// </summary>
    [TestFixture]
    public class SubscriptionManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Subscription> _testSubscriptions = new List<Subscription>();
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
            var subscriptionDrafts = Helper.GetTestSubscriptionsDrafts();
            Assert.NotNull(subscriptionDrafts, "Subscription drafts null");
            foreach (var subscriptionDraft in subscriptionDrafts)
            {
                var subscriptionTask = _client.Subscriptions().CreateSubscriptionAsync(subscriptionDraft);
                subscriptionTask.Wait();
                Assert.IsTrue(subscriptionTask.Result.Success);
                _testSubscriptions.Add(subscriptionTask.Result.Result);
            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (var subscription in _testSubscriptions)
            {
                _client.Subscriptions().DeleteSubscriptionAsync(subscription);
            }
        }

        /// <summary>
        /// Tests the SubscriptionManager.GetSubscriptionByIdAsync method.
        /// </summary>
        /// <see cref="SubscriptionManager.GetSubscriptionByIdAsync"/>
        [Test]
        public async Task ShouldGetSubscriptionByIdAsync()
        {
            Assert.NotNull(_testSubscriptions);
            if (_testSubscriptions.Count == 0)
            {
                Console.Error.WriteLine("WARNING: ShouldGetSubscriptionByIdAsync - No valid subscriptions have been setup. Check that destination configuration(s) have been set up in App.Config.");
                return;
            }
            SubscriptionManager manager = new SubscriptionManager(_client);
            foreach (var subscription in _testSubscriptions)
            {
                var result = await manager.GetSubscriptionByIdAsync(subscription.Id);
                Assert.NotNull(result);
                var resultSubscription = result.Result;
                Assert.NotNull(resultSubscription);
                Assert.NotNull(resultSubscription.Id);
                Assert.NotNull(resultSubscription.Key);
                Assert.NotNull(resultSubscription.Destination);
                Assert.NotNull(resultSubscription.Destination.Type);
                switch (resultSubscription.Destination.Type)
                {
                    case "IronMQ":
                        Assert.IsInstanceOf(typeof(IronMQDestination), resultSubscription.Destination);
                        var destIronMQ = resultSubscription.Destination as IronMQDestination;
                        Assert.NotNull(destIronMQ.Type);
                        Assert.NotNull(destIronMQ.Uri);
                        break;
                    case "SQS":
                        Assert.IsInstanceOf(typeof(AWSSQSDestination), resultSubscription.Destination);
                        var destSQS = resultSubscription.Destination as AWSSQSDestination;
                        Assert.NotNull(destSQS.Type);
                        Assert.NotNull(destSQS.AccessKey);
                        Assert.NotNull(destSQS.AccessSecret);
                        Assert.NotNull(destSQS.QueueUrl);
                        Assert.NotNull(destSQS.Region);
                        break;
                    case "SNS":
                        Assert.IsInstanceOf(typeof(AWSSNSDestination), resultSubscription.Destination);
                        var destSNS = resultSubscription.Destination as AWSSNSDestination;
                        Assert.NotNull(destSNS.Type);
                        Assert.NotNull(destSNS.AccessKey);
                        Assert.NotNull(destSNS.AccessSecret);
                        Assert.NotNull(destSNS.TopicArn);
                        break;
                    default:
                        throw new Exception(string.Format("Unknown destination type: {0}", resultSubscription.Destination.Type));
                }
            }
        }
        
        /// <summary>
        /// Tests the SubscriptionManager.GetSubscriptionByKeyAsync method.
        /// </summary>
        /// <see cref="SubscriptionManager.GetSubscriptionByKeyAsync"/>
        [Test]
        public async Task ShouldGetSubscriptionByKey()
        {
            Assert.NotNull(_testSubscriptions);
            if (_testSubscriptions.Count == 0)
            {
                Console.Error.WriteLine("WARNING: GetSubscriptionByKeyAsync - No valid subscriptions have been setup. Check that destination configuration(s) have been set up in App.Config.");
                return;
            }
            SubscriptionManager manager = new SubscriptionManager(_client);
            foreach (var subscription in _testSubscriptions)
            {
                var result = await manager.GetSubscriptionByKeyAsync(subscription.Key);
                Assert.NotNull(result);
                var resultSubscription = result.Result;
                Assert.NotNull(resultSubscription);
                Assert.NotNull(resultSubscription.Id);
                Assert.NotNull(resultSubscription.Key);
                Assert.NotNull(resultSubscription.Destination);
                Assert.NotNull(resultSubscription.Destination.Type);
                switch ((string)resultSubscription.Destination.Type)
                {
                    case "IronMQ":
                        Assert.IsInstanceOf(typeof(IronMQDestination), resultSubscription.Destination);
                        var destIronMQ = resultSubscription.Destination as IronMQDestination;
                        Assert.NotNull(destIronMQ.Type);
                        Assert.NotNull(destIronMQ.Uri);
                        break;
                    case "SQS":
                        Assert.IsInstanceOf(typeof(AWSSQSDestination), resultSubscription.Destination);
                        var destSQS = resultSubscription.Destination as AWSSQSDestination;
                        Assert.NotNull(destSQS.Type);
                        Assert.NotNull(destSQS.AccessKey);
                        Assert.NotNull(destSQS.AccessSecret);
                        Assert.NotNull(destSQS.QueueUrl);
                        Assert.NotNull(destSQS.Region);
                        break;
                    case "SNS":
                        Assert.IsInstanceOf(typeof(AWSSNSDestination), resultSubscription.Destination);
                        var destSNS = resultSubscription.Destination as AWSSNSDestination;
                        Assert.NotNull(destSNS.Type);
                        Assert.NotNull(destSNS.AccessKey);
                        Assert.NotNull(destSNS.AccessSecret);
                        Assert.NotNull(destSNS.TopicArn);
                        break;
                    default:
                        throw new Exception(string.Format("Unknown destination type: {0}", resultSubscription.Destination.Type));
                }
            }
        }

    }
}