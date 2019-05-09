using System;
using commercetools.Sdk.Domain.ProductDiscounts;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Errors
{
    public class ErrorsFixture : ClientFixture, IDisposable
    {
        public CategoryFixture CategoryFixture { get; private set; }

        public ErrorsFixture(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            this.CategoryFixture = new CategoryFixture(diagnosticMessageSink);
        }
        public void Dispose()
        {
            this.CategoryFixture.Dispose();
        }
    }
}
