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

        /// <summary>
        /// unPublish Products Before Dispose
        /// </summary>
        private void UnPublishProducts()
        {
            var publishedProducts = productFixture.ProductsToDelete.Where(p => p.MasterData.Published).ToList();
            if (publishedProducts.Count > 0)
            {
                foreach (var product in publishedProducts)
                {
                    productFixture.Unpublish(product);
                }
            }
        }

    }
}
