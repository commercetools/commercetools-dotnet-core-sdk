using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.HttpApi.IntegrationTests.Products;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ProductProjections
{
    public class ProductProjectionsFixture : ClientFixture, IDisposable
    {
        public readonly ProductFixture productFixture;

        public ProductProjectionsFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.productFixture = new ProductFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.productFixture.Dispose();
        }

    }
}
