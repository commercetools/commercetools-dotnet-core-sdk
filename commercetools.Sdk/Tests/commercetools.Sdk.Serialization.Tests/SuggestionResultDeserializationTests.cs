using System.Linq;
using commercetools.Sdk.Domain.Suggestions;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class SuggestionResultDeserializationTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public SuggestionResultDeserializationTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

        [Fact]
        public void ProductSuggestionResultDeserialization()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = @"{
                ""searchKeywords.en"":[{""text"":""Multi tool""}]
            }";
            var suggestionResult = serializerService.Deserialize<SuggestionResult<ProductSuggestion>>(serialized);
            Assert.IsType<SuggestionResult<ProductSuggestion>>(suggestionResult);
            Assert.Single(suggestionResult.Suggestions.Keys);
            Assert.Equal("en", suggestionResult.Suggestions.Keys.ElementAt(0));
        }

        [Fact]
        public void ProductSuggestionResultTwoLanguagesDeserialization()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = @"{
                ""searchKeywords.de"":[{""text"":""Schweizer Messer""}],
                ""searchKeywords.en"":[{""text"":""Multi tool""}]
            }";
            var suggestionResult = serializerService.Deserialize<SuggestionResult<ProductSuggestion>>(serialized);
            Assert.IsType<SuggestionResult<ProductSuggestion>>(suggestionResult);
            Assert.Equal(2, suggestionResult.Suggestions.Keys.Count);
            Assert.Equal("de", suggestionResult.Suggestions.Keys.ElementAt(0));
            Assert.Equal("en", suggestionResult.Suggestions.Keys.ElementAt(1));
        }
    }
}
