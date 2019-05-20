using System;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.Zones;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ShippingMethods
{
    [Collection("Integration Tests")]
    public class ShippingMethodIntegrationTests : IClassFixture<ServiceProviderFixture>, IDisposable
    {
        private readonly ShippingMethodsFixture shippingMethodsFixture;

        public ShippingMethodIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.shippingMethodsFixture = new ShippingMethodsFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.shippingMethodsFixture.Dispose();
        }

        [Fact]
        public void CreateShippingMethod()
        {
            IClient commerceToolsClient = this.shippingMethodsFixture.GetService<IClient>();
            ShippingMethodDraft shippingMethodDraft = this.shippingMethodsFixture.GetShippingMethodDraft();
            ShippingMethod shippingMethod = commerceToolsClient
                .ExecuteAsync(new CreateCommand<ShippingMethod>(shippingMethodDraft)).Result;
            this.shippingMethodsFixture.ShippingMethodsToDelete.Add(shippingMethod);
            Assert.Equal(shippingMethodDraft.Name, shippingMethod.Name);
        }
    }
}
