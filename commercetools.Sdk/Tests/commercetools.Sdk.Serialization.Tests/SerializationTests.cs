using System.Collections.Generic;
using System.IO;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Registration;
using FluentAssertions.Json;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
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
        public void SerializationWithoutValidation()
        {
            var services = new ServiceCollection();
            services.UseRegistration();
            services.UseDomain();
            services.UseSerialization();
            var serviceProvider = services.BuildServiceProvider();
            var serializerService = serviceProvider.GetService<ISerializerService>();

            ReviewDraft reviewDraft = new ReviewDraft()
            {
                Locale = "en-ZZZ"
            };
            var draft = serializerService.Serialize(reviewDraft);
            Assert.Equal("{\"locale\":\"en-ZZZ\"}", draft);
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
            Assert.Single(exception.Errors);
        }

        [Fact]
        public void SerializeReviewDraftInvalidCurrency()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            CartDraft cartDraft = new CartDraft()
            {
                Currency = "ZZZ"
            };
            ValidationException exception = Assert.Throws<ValidationException>(() => serializerService.Serialize(cartDraft));
            Assert.Single(exception.Errors);
        }

        [Fact]
        public void SerializeReviewDraftInvalidCountry()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            CartDraft cartDraft = new CartDraft()
            {
                Currency = "EUR",
                Country = "ZZ"
            };
            ValidationException exception = Assert.Throws<ValidationException>(() => serializerService.Serialize(cartDraft));
            Assert.Single(exception.Errors);
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

        [Fact]
        public void ResourceIdentifiersDeserialization()
        {
            //Deserialize 2 of resourceIdentifiers to list of resourceIdentifiers with the correct instance type based on Type Id
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Types/References.json");
            List<ResourceIdentifier> references = serializerService.Deserialize<List<ResourceIdentifier>>(serialized);
            Assert.IsType<Reference<Category>>(references[0]);
            Assert.IsType<Reference<Product>>(references[1]);

            string serializedRev = File.ReadAllText("Resources/Types/Review.json");
            Review review = serializerService.Deserialize<Review>(serializedRev);
            Assert.IsType<Reference<Product>>(review.Target);

        }

        [Fact]
        public void ResourceIdentifierSerialization()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            ResourceIdentifier<ProductType> productType = new ResourceIdentifier<ProductType>
            {
                Key = "Key12",
                Id = "1"
            };
            string result = serializerService.Serialize(productType);
            JToken resultFormatted = JValue.Parse(result);
            string serialized = File.ReadAllText("Resources/Types/ResourceIdentifier.json");
            JToken serializedFormatted = JValue.Parse(serialized);
            serializedFormatted.Should().BeEquivalentTo(resultFormatted);
        }

        [Fact]
        public void ProductDraftSerializationUsingResourceIdentifier()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;

            var productDraft = new ProductDraft
            {
                Name = new LocalizedString() {{"en", "name"}},
                Slug = new LocalizedString() {{"en", "slug"}},
                ProductType = new ResourceIdentifier<ProductType>
                {
                    Key = "ProductTypeKey"
                },
                TaxCategory= new ResourceIdentifier<TaxCategory>
                {
                    Key = "TaxCategoryKey"
                },
                Categories = new List<IReference<Category>>()
                {
                    new ResourceIdentifier<Category>
                    {
                        Key = "CategoryKey"
                    }
                },
                State = new ResourceIdentifier<State>
                {
                    Key = "StateKey"
                }
            };

            string result = serializerService.Serialize(productDraft);
            JToken resultFormatted = JValue.Parse(result);
            string serialized = File.ReadAllText("Resources/Types/ProductDraftWithResourceIdentifier.json");
            JToken serializedFormatted = JValue.Parse(serialized);
            serializedFormatted.Should().BeEquivalentTo(resultFormatted);
        }

        [Fact]
        public void ProductDraftSerializationUsingReference()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;

            var productDraft = new ProductDraft
            {
                Name = new LocalizedString() {{"en", "name"}},
                Slug = new LocalizedString() {{"en", "slug"}},
                ProductType = new Reference<ProductType>
                {
                    Id = "ProductTypeId"
                },
                TaxCategory= new Reference<TaxCategory>
                {
                    Id = "TaxCategoryId"
                },
                Categories = new List<IReference<Category>>()
                {
                    new Reference<Category>
                    {
                        Id = "CategoryId"
                    }
                },
                State = new Reference<State>
                {
                    Id = "StateId"
                }
            };

            string result = serializerService.Serialize(productDraft);
            JToken resultFormatted = JValue.Parse(result);
            string serialized = File.ReadAllText("Resources/Types/ProductDraftWithReference.json");
            JToken serializedFormatted = JValue.Parse(serialized);
            serializedFormatted.Should().BeEquivalentTo(resultFormatted);
        }
    }
}
