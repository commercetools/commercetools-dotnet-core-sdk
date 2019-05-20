using System;
using commercetools.Sdk.Domain.ProductDiscounts;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Errors
{
    public class ErrorsFixture : ClientFixture, IDisposable
    {
        public CategoryFixture CategoryFixture { get; private set; }

        public ErrorsFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.CategoryFixture = new CategoryFixture(serviceProviderFixture);
        }
        public void Dispose()
        {
            this.CategoryFixture.Dispose();
        }
    }
}
