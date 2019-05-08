using System;
using commercetools.Sdk.Domain.ProductDiscounts;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Errors
{
    public class ErrorsFixture : ClientFixture, IDisposable
    {
        public CategoryFixture CategoryFixture { get; private set; }

        public ErrorsFixture() :base()
        {
            this.CategoryFixture = new CategoryFixture();
        }
        public void Dispose()
        {
            this.CategoryFixture.Dispose();
        }
    }
}