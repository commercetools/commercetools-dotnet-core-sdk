using System.IO;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.ShippingMethods;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class DeserializationTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public DeserializationTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

        [Fact]
        public void DeserializeLocalizedString()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;

            string serialized = @"
                {
                    ""name"": {
                        ""en"": ""name-en"",
                        ""en-US"": ""name-en-us""
                    }
                }
            ";

            var product = serializerService.Deserialize<ProductProjection>(serialized);

            Assert.Equal("name-en", product.Name["en"]);
            Assert.Equal("name-en-us", product.Name["en-US"]);
        }
        
        [Fact]
        public void DeserializeShippingMethods()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;

            var serialized = File.ReadAllText("Resources/ShippingMethods/ShippingMethodsAsPageQueryResult.json");
            var shippingMethodsResult = serializerService.Deserialize<PagedQueryResult<ShippingMethod>>(serialized);

            Assert.Equal(2, shippingMethodsResult.Count);
            Assert.Equal(20, shippingMethodsResult.Limit);
            Assert.Equal("7806c94b-bd59-47e0-b4b4-fd32ecd2f93d", shippingMethodsResult.Results[0].Id);
            Assert.Equal(ReferenceTypeId.TaxCategory, shippingMethodsResult.Results[0].TaxCategory.TypeId);
            Assert.Equal("3d018013-b53d-422a-933c-7294cf114323", shippingMethodsResult.Results[0].TaxCategory.Obj.Id);
        }
        
        [Fact]
        public void DeserializeShippingMethodsList()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;

            var serialized = File.ReadAllText("Resources/ShippingMethods/ShippingMethodsAsList.json");
            var shippingMethodsResult = serializerService.Deserialize<PagedQueryResult<ShippingMethod>>(serialized);

            Assert.Equal(2, shippingMethodsResult.Count);
            Assert.Equal("7806c94b-bd59-47e0-b4b4-fd32ecd2f93d", shippingMethodsResult.Results[0].Id);
            Assert.Equal(ReferenceTypeId.TaxCategory, shippingMethodsResult.Results[0].TaxCategory.TypeId);
            Assert.Equal("3d018013-b53d-422a-933c-7294cf114323", shippingMethodsResult.Results[0].TaxCategory.Obj.Id);
        }

        [Fact]
        public void DeserializeCustomField()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;

            string serialized = @"
                {
                    ""fields"": {
                        ""foo"": ""bar"",
                        ""Foos"": ""Bars""
                    }
                }
            ";

            var customFields = serializerService.Deserialize<CustomFields>(serialized);

            Assert.Equal("bar", customFields.Fields["foo"]);
            Assert.Equal("Bars", customFields.Fields["Foos"]);
        }

    }
}
