using commercetools.Sdk.Client;
using Xunit;


namespace commercetools.Sdk.IntegrationTests.Subscriptions
{
    [Collection("Integration Tests")]
    public class SubscriptionIntegrationTests
    {
        private readonly IClient client;

        public SubscriptionIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }
    }
}
