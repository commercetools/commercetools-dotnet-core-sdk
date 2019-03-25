using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ProductProjections
{
    public class ProductProjectionsFixture : ClientFixture, IDisposable
    {
        public readonly ProductFixture productFixture;

        public ProductProjectionsFixture() : base()
        {
            this.productFixture = new ProductFixture();
        }

        public void Dispose()
        {
            this.productFixture.Dispose();
        }

    }
}
