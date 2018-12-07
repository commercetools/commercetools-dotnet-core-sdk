using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Attributes;
using commercetools.Sdk.Domain.Validation;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class SerializationTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public SerializationTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

        [Fact]
        public void SerializeReviewDraftInvalidLocale()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            ReviewDraft reviewDraft = new ReviewDraft()
            {
                Locale = "en-ZZZ"
            };
            ValidationException exception = Assert.Throws<ValidationException>(() => serializerService.Serialize(reviewDraft));
            Assert.Single(exception.ValidationResults);
        }

       
    }
}