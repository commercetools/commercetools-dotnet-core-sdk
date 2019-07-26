using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;

namespace commercetools.Sdk.IntegrationTests.ProductSuggestions
{
    public static class ProductSuggestionsFixture
    {
        #region DraftBuilds
        public static ProductDraft DefaultProductDraftWithSearchKeywords(ProductDraft draft)
        {
            var productDraft = DefaultProductDraft(draft);
            productDraft.SearchKeywords = GetSearchKeywords();
            return productDraft;
        }
        #endregion

        #region WithSuggestProduct

        public static async Task WithSuggestProduct( IClient client, Action<Product> func)
        {
            await WithProduct(client,DefaultProductDraftWithSearchKeywords, func);
        }
        public static async Task WithSuggestProduct( IClient client, Func<Product, Task> func)
        {
            await WithProduct(client,DefaultProductDraftWithSearchKeywords, func);
        }
        #endregion

        private static Dictionary<string, List<SearchKeywords>> GetSearchKeywords()
        {
            var searchKeywords = new Dictionary<string, List<SearchKeywords>>();
            var englishSearchKeywordsList = GetEnglishSearchKeywordList();
            var germanySearchKeywordsList = GetGermanySearchKeywordList();
            searchKeywords.Add("en", englishSearchKeywordsList);
            searchKeywords.Add("de", germanySearchKeywordsList);
            return searchKeywords;
        }

        private static List<SearchKeywords> GetEnglishSearchKeywordList()
        {
            var searchKeywordsList = new List<SearchKeywords>
            {
                new SearchKeywords {Text = "Multi tool"},
                new SearchKeywords {Text = "Swiss Army Knife", SuggestTokenizer = new WhitespaceTokenizer()}
            };

            return searchKeywordsList;
        }
        private static List<SearchKeywords> GetGermanySearchKeywordList()
        {
            var searchKeywordsList = new List<SearchKeywords>
            {
                new SearchKeywords {Text = "Schweizer Messer", SuggestTokenizer = GetCustomTokenizer()}
            };

            return searchKeywordsList;
        }

        private static SuggestTokenizer GetCustomTokenizer()
        {
            var suggestTokenizer = new CustomTokenizer();
            suggestTokenizer.Inputs = new List<string>
            {
                "schweizer messer",
                "offiziersmesser",
                "sackmesser"
            };
            return suggestTokenizer;
        }
    }
}
