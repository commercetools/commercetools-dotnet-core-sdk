using System.Collections.Generic;
using System.IO;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Projects;
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

        [Fact]
        public void DeserializeExponentialRangeFacet()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            
            string serialized = @"
                {
                     ""type"":""range"",
                     ""dataType"": ""number"",
                     ""ranges"": [{
                        ""type"":""double"",
                        ""from"":0.0,
                        ""fromStr"":""0.0"",
                        ""to"":0.0,
                        ""toStr"":""0.0"",
                        ""count"":119104,
                        ""totalCount"":119104,
                        ""total"":3.715960759E9,
                        ""min"":0.0,
                        ""max"":2301000.0,
                        ""mean"":31199.294389777002
                    }]
                }
            ";

            var facetResult = serializerService.Deserialize<FacetResult>(serialized);

            Assert.Equal(3715960759, (facetResult as RangeFacetResult).Ranges.First().Total);
        }
        
        [Fact]
        public void DeserializeProject()
        {
            var serializerService = this.serializationFixture.SerializerService;

            var serialized = File.ReadAllText("Resources/Projects/project.json");
            var project = serializerService.Deserialize<Project>(serialized);
            
            Assert.NotNull(project);
            Assert.NotNull(project.SearchIndexing);
            Assert.Equal(SearchIndexingConfigurationStatus.Deactivated, project.SearchIndexing.Products.Status);
            Assert.Null(project.SearchIndexing.Orders);
        }
    }
}
