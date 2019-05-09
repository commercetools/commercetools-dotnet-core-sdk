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

        public ProductProjectionsFixture(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            this.productFixture = new ProductFixture(diagnosticMessageSink);
        }

        public void Dispose()
        {
            this.productFixture.Dispose();
        }

    }
}
