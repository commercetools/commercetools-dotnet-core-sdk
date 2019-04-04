using System.Collections.Generic;
using System.IO;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Validation;
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

        [Fact]
        public void ReferenceDeserialization()
        {
            //Deserialize 2 of references to list of references with the correct instance type based on Type Id
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Types/References.json");
            List<Reference> references = serializerService.Deserialize<List<Reference>>(serialized);
            Assert.IsType<Reference<Category>>(references[0]);
            Assert.IsType<Reference<Product>>(references[1]);

            string serializedRev = File.ReadAllText("Resources/Types/Review.json");
            Review review = serializerService.Deserialize<Review>(serializedRev);
            Assert.IsType<Reference<Product>>(review.Target);

            var res = new ResourceIdentifier<Product>();
            Assert.Equal(ReferenceTypeId.Product, res.TypeId);
        }
    }
}
