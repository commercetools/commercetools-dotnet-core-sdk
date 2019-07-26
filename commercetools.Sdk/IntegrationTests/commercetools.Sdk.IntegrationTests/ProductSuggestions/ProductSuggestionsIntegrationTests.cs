using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Suggestions;
using static commercetools.Sdk.IntegrationTests.ProductSuggestions.ProductSuggestionsFixture;
using Xunit;

namespace commercetools.Sdk.IntegrationTests.ProductSuggestions
{
    [Collection("Integration Tests")]
    public class ProductSuggestionsIntegrationTests
    {
        private readonly IClient client;

        public ProductSuggestionsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task ProductSuggestWithNoTokenizer()
        {
            await WithSuggestProduct(client, async product =>
            {
                Assert.NotEmpty(product.MasterData.Staged.SearchKeywords);
                Assert.Equal(2, product.MasterData.Staged.SearchKeywords.Keys.Count);
                Assert.Equal(2, product.MasterData.Staged.SearchKeywords["en"].Count);
                var multiToolSearchKeyword = product.MasterData.Staged.SearchKeywords["en"]
                    .FirstOrDefault(keyword => keyword.Text.StartsWith("Multi"));
                Assert.NotNull(multiToolSearchKeyword);

                //Arrange
                var searchKeyword = new LocalizedString {{"en", "multi"}};
                var suggestParams = new SuggestQueryCommandParameters(searchKeyword);
                var suggestCommand = new SuggestQueryCommand<ProductSuggestion>(suggestParams);

                //Act
                var suggestionResult = await client.ExecuteAsync(suggestCommand);

                //Assert
                Assert.Single(suggestionResult.Suggestions);
                var productSuggestions = suggestionResult.Suggestions["en"];
                Assert.Single(productSuggestions);
                Assert.Equal(multiToolSearchKeyword.Text, productSuggestions[0].Text);
            });
        }

        [Fact]
        public async Task ProductSuggestWithNoTokenizerWithNoResults()
        {
            await WithSuggestProduct(client, async product =>
            {
                Assert.NotEmpty(product.MasterData.Staged.SearchKeywords);
                Assert.Equal(2, product.MasterData.Staged.SearchKeywords.Keys.Count);
                Assert.Equal(2, product.MasterData.Staged.SearchKeywords["en"].Count);
                var multiToolSearchKeyword = product.MasterData.Staged.SearchKeywords["en"]
                    .FirstOrDefault(keyword => keyword.Text.StartsWith("Multi"));
                Assert.NotNull(multiToolSearchKeyword);

                //Arrange
                var searchKeyword = new LocalizedString {{"en", "tool"}};
                var suggestParams = new SuggestQueryCommandParameters(searchKeyword);
                var suggestCommand = new SuggestQueryCommand<ProductSuggestion>(suggestParams);

                //Act
                var suggestionResult = await client.ExecuteAsync(suggestCommand);

                //Assert
                Assert.Single(suggestionResult.Suggestions);
                Assert.Empty(suggestionResult.Suggestions["en"]);
            });
        }

        [Fact]
        public async Task ProductSuggestWithWhitespaceTokenizer()
        {
            await WithSuggestProduct(client, async product =>
            {
                Assert.NotEmpty(product.MasterData.Staged.SearchKeywords);
                Assert.Equal(2, product.MasterData.Staged.SearchKeywords.Keys.Count);
                Assert.Equal(2, product.MasterData.Staged.SearchKeywords["en"].Count);
                var swissArmyKnifeSearchKeyword = product.MasterData.Staged.SearchKeywords["en"]
                    .FirstOrDefault(keyword =>
                        keyword.SuggestTokenizer != null &&
                        keyword.SuggestTokenizer.GetType() == typeof(WhitespaceTokenizer));

                Assert.NotNull(swissArmyKnifeSearchKeyword);

                //Arrange
                var searchKeyword = new LocalizedString {{"en", "kni"}};
                var suggestParams = new SuggestQueryCommandParameters(searchKeyword);
                var suggestCommand = new SuggestQueryCommand<ProductSuggestion>(suggestParams);

                //Act
                var suggestionResult = await client.ExecuteAsync(suggestCommand);

                //Assert
                Assert.Single(suggestionResult.Suggestions);
                var productSuggestions = suggestionResult.Suggestions["en"];
                Assert.Single(productSuggestions);
                Assert.Equal(swissArmyKnifeSearchKeyword.Text, productSuggestions[0].Text);
            });
        }

        [Fact]
        public async Task ProductSuggestWithCustomTokenizer()
        {
            await WithSuggestProduct(client, async product =>
            {
                Assert.NotEmpty(product.MasterData.Staged.SearchKeywords);
                Assert.Equal(2, product.MasterData.Staged.SearchKeywords.Keys.Count);
                Assert.Single(product.MasterData.Staged.SearchKeywords["de"]);
                var schweizerSearchKeyword = product.MasterData.Staged.SearchKeywords["de"].FirstOrDefault();

                Assert.NotNull(schweizerSearchKeyword);

                //Arrange
                var searchKeyword = new LocalizedString {{"de", "offiz"}};
                var suggestParams = new SuggestQueryCommandParameters(searchKeyword);
                var suggestCommand = new SuggestQueryCommand<ProductSuggestion>(suggestParams);

                //Act
                var suggestionResult = await client.ExecuteAsync(suggestCommand);

                //Assert
                Assert.Single(suggestionResult.Suggestions);
                var productSuggestions = suggestionResult.Suggestions["de"];
                Assert.Single(productSuggestions);
                Assert.Equal(schweizerSearchKeyword.Text, productSuggestions[0].Text);
            });
        }

        [Fact]
        public async Task ProductSuggestWithTwoLanguages()
        {
            await WithSuggestProduct(client, async product =>
            {
                Assert.NotEmpty(product.MasterData.Staged.SearchKeywords);
                Assert.Equal(2, product.MasterData.Staged.SearchKeywords.Keys.Count);
                Assert.Equal(2, product.MasterData.Staged.SearchKeywords["en"].Count);
                Assert.Single(product.MasterData.Staged.SearchKeywords["de"]);

                var multiToolSearchKeyword = product.MasterData.Staged.SearchKeywords["en"]
                    .FirstOrDefault(keyword => keyword.Text.StartsWith("Multi"));
                var schweizerSearchKeyword = product.MasterData.Staged.SearchKeywords["de"].FirstOrDefault();

                Assert.NotNull(multiToolSearchKeyword);
                Assert.NotNull(schweizerSearchKeyword);

                //Arrange
                var searchKeyword = new LocalizedString {{"de", "offiz"}, {"en", "multi"}};
                var suggestParams = new SuggestQueryCommandParameters(searchKeyword);
                var suggestCommand = new SuggestQueryCommand<ProductSuggestion>(suggestParams);

                //Act
                var suggestionResult = await client.ExecuteAsync(suggestCommand);

                //Assert
                Assert.Equal(2,suggestionResult.Suggestions.Count);

                var englishProductSuggestions = suggestionResult.Suggestions["en"];
                var germanyProductSuggestions = suggestionResult.Suggestions["de"];

                Assert.Single(englishProductSuggestions);
                Assert.Single(germanyProductSuggestions);
                Assert.Equal(schweizerSearchKeyword.Text, germanyProductSuggestions[0].Text);
                Assert.Equal(multiToolSearchKeyword.Text, englishProductSuggestions[0].Text);
            });
        }
    }
}
