using System;
using commercetools.Sdk.Client;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Reviews
{
    [Collection("Integration Tests")]
    public class ReviewIntegrationTests : IClassFixture<ServiceProviderFixture>, IDisposable
    {
        private readonly ReviewFixture reviewFixture;

        public ReviewIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.reviewFixture = new ReviewFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.reviewFixture.Dispose();
        }

    }
}
