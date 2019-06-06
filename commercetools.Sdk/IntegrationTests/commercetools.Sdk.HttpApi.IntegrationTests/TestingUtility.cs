using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Products.Attributes;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;
using LocalizedEnumValue = commercetools.Sdk.Domain.Common.LocalizedEnumValue;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public static class TestingUtility
    {
        #region Fields

        private static readonly Random Random = new Random();

        private static readonly List<string> EuropeCountries = new List<string>()
            {"BE", "BG", "HR", "CY", "CZ", "DK", "EE", "FR", "HU", "IT", "IE", "NL", "PL", "PT", "ES", "SE"};

        public static readonly string ExternalImageUrl = "https://commercetools.com/wp-content/uploads/2018/06/Feature_Guide.png";
        public static readonly string AssetUrl = "https://commercetools.com/wp-content/uploads/2018/07/rewe-logo-1.gif";
        public const string DefaultContainerName = "CustomObjectFixtures";
        #endregion

        #region Functions

        public static string RandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static int RandomInt(int? min = null, int? max = null)
        {
            int ran;
            if (min.HasValue && max.HasValue)
            {
                ran = Random.Next(min.Value, max.Value);
            }
            else
            {
                ran = Math.Abs(Random.Next());
            }

            return ran;
        }

        public static string RandomDoubleAsString(double min, double max)
        {
            var ran = Random.NextDouble(0.1, 0.9);
            return string.Format(CultureInfo.InvariantCulture, "{0:0.00}", ran);
        }

        public static Double RandomDouble()
        {
            var ran = Random.NextDouble(0.1, 0.9);
            return ran;
        }

        public static Double RandomDouble(double min, double max)
        {
            var ran = Random.NextDouble(min, max);
            return ran;
        }

        public static string RandomSortOrder()
        {
            int append = 5; //hack to not have a trailing 0 which is not accepted in sphere
            return "0." + RandomInt() + append;
        }

        public static string GetRandomEuropeCountry()
        {
            int ran = RandomInt(0, EuropeCountries.Count);
            string country = EuropeCountries[ran];
            return country;
        }

        public static Money MultiplyMoney(BaseMoney oldMoney, int multiplyBy)
        {
            var newMoney = new Money()
            {
                CurrencyCode = oldMoney.CurrencyCode,
                FractionDigits = oldMoney.FractionDigits,
                CentAmount = oldMoney.CentAmount * multiplyBy
            };
            return newMoney;
        }

        public static List<LocalizedEnumValue> GetCartClassificationTestValues()
        {
            var classificationValues = new List<LocalizedEnumValue>
            {
                new LocalizedEnumValue()
                {
                    Key = "Small", Label = new LocalizedString() {{"en", "Small"}, {"de", "Klein"}}
                },
                new LocalizedEnumValue()
                {
                    Key = "Medium", Label = new LocalizedString() {{"en", "Medium"}, {"de", "Mittel"}}
                },
                new LocalizedEnumValue()
                {
                    Key = "Heavy", Label = new LocalizedString() {{"en", "Heavy"}, {"de", "Schwergut"}}
                }
            };
            return classificationValues;
        }
         /// <summary>
        /// Get Random Product Variant Draft with attributes
        /// </summary>
        /// <param name="referenceAttributeId"></param>
        /// <param name="referenceTypeId"></param>
        /// <returns></returns>
        public static ProductVariantDraft GetRandomProductVariantDraft(string referenceAttributeId = "",
            ReferenceTypeId? referenceTypeId = null)
        {
            var productVariantDraft = new ProductVariantDraft()
            {
                Key = RandomString(10),
                Sku = RandomString(10),
                Prices = GetRandomListOfPriceDraft(),//two prices
                Attributes = GetListOfRandomAttributes(referenceAttributeId, referenceTypeId)
            };
            return productVariantDraft;
        }

        /// <summary>
        /// Get Random Price Draft
        /// </summary>
        /// <returns></returns>
        public static PriceDraft GetRandomPriceDraft()
        {
            var randomAmount = RandomInt(1000, 10000);
            var priceDraft = GetPriceDraft(randomAmount, DateTime.Now, DateTime.Now.AddMonths(1));
            return priceDraft;
        }

        public static Price GetRandomPrice()
        {
            var randomAmount = RandomInt(1000, 10000);
            var price = GetPrice(randomAmount);
            return price;
        }

        public static PriceDraft GetPriceDraft(int centAmount,  DateTime? validFrom = null, DateTime? validUntil = null, string currency = "EUR")
        {
            var money = new Money()
            {
                CentAmount = centAmount,
                CurrencyCode = currency
            };
            var priceDraft = new PriceDraft()
            {
                Value = money,
                ValidFrom = validFrom,
                ValidUntil = validUntil
            };
            return priceDraft;
        }

        public static Price GetPrice(int centAmount,  DateTime? validFrom = null, DateTime? validUntil = null, string currency = "EUR")
        {
            var money = new Money()
            {
                CentAmount = centAmount,
                CurrencyCode = currency
            };
            var price = new Price()
            {
                Value = money,
                ValidFrom = validFrom,
                ValidUntil = validUntil
            };
            return price;
        }

        public static List<PriceDraft> GetRandomListOfPriceDraft(int count = 2)
        {
            List<PriceDraft> prices = new List<PriceDraft>();

            //first Add valid price for now
            prices.Add(GetPriceDraft(RandomInt(1000, 10000)));
            //then add future prices
            for (int i = 2; i <= count; i++)
            {
                prices.Add(GetPriceDraft(RandomInt(1000, 10000), DateTime.Now.AddMonths(i), DateTime.Now.AddMonths(i+1)));
            }

            return prices;
        }

        /// <summary>
        /// Get list of Random attributes with reference attribute if passed
        /// </summary>
        /// <param name="referenceAttributeId"></param>
        /// <param name="referenceTypeId"></param>
        /// <returns></returns>
        public static List<Attribute> GetListOfRandomAttributes(string referenceAttributeId = "",
            ReferenceTypeId? referenceTypeId = null)
        {
            List<Attribute> attributes = new List<Attribute>();
            attributes.Add(new TextAttribute() {Name = "text-attribute-name", Value = RandomString(10)});
            attributes.Add(new LocalizedTextAttribute()
            {
                Name = "localized-text-attribute-name", Value = new LocalizedString() {{"en", RandomString(10)}}
            });
            attributes.Add(new EnumAttribute()
                {Name = "enum-attribute-name", Value = new PlainEnumValue() {Key = "enum-key-1"}});
            attributes.Add(new LocalizedEnumAttribute()
                {Name = "localized-enum-attribute-name", Value = new commercetools.Sdk.Domain.Products.Attributes.LocalizedEnumValue() {Key = "enum-key-1"}});
            attributes.Add(new BooleanAttribute() {Name = "boolean-attribute-name", Value = true});
            attributes.Add(new NumberAttribute() {Name = "number-attribute-name", Value = 10});
            attributes.Add(new DateTimeAttribute()
                {Name = "date-time-attribute-name", Value = new DateTime(2018, 12, 10, 23, 43, 02)});
            attributes.Add(new DateAttribute() {Name = "date-attribute-name", Value = new DateTime(2018, 12, 10)});
            attributes.Add(new TimeAttribute() {Name = "time-attribute-name", Value = new TimeSpan(23, 43, 10)});
            attributes.Add(new MoneyAttribute()
                {Name = "money-attribute-name", Value = new Money() {CentAmount = 4000, CurrencyCode = "EUR"}});
            if (!string.IsNullOrEmpty(referenceAttributeId) && referenceTypeId != null)
            {
                attributes.Add(new ReferenceAttribute()
                {
                    Name = "reference-attribute-name",
                    Value = new Reference<Category>()
                    {
                        Id = referenceAttributeId
                    }
                });
            }

            SetTextAttribute setAttribute = new SetTextAttribute();
            AttributeSet<string> stringSet = new AttributeSet<string>() {"test1", "test2"};
            setAttribute.Value = stringSet;
            setAttribute.Name = "set-text-attribute-name";
            attributes.Add(setAttribute);
            return attributes;
        }

        /// <summary>
        /// Get list of Images
        /// </summary>
        /// <param name="count">count of images</param>
        /// <returns></returns>
        public static List<Image> GetListOfImages(int count = 2)
        {
            var images = new List<Image>();
            for (int i = 1; i<= count; i++)
            {
                images.Add(new Image
                {
                    Label = $"Test-Image-{i}",
                    Url = $"http://www.commercetools.com/assets/img/logo_{i}.gif",
                    Dimensions = new Dimensions { W = 50, H = 50}
                });
            }
            return images;
        }

        /// <summary>
        /// Get list of Asset Drafts
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<AssetDraft> GetListOfAssetsDrafts(int count = 2)
        {
            var assets = new List<AssetDraft>();
            for (int i = 1; i<= count; i++)
            {
                assets.Add(GetAssetDraft());
            }
            return assets;
        }

        public static AssetDraft GetAssetDraft()
        {
            var rand = TestingUtility.RandomInt();
            var assetSource = GetAssetSource();

            var asset = new AssetDraft()
            {
                Key = $"Asset-Key-{rand}",
                Sources = new List<AssetSource> {assetSource},
                Name = new LocalizedString() {{"en", $"Asset_Name_{rand}"}},
                Description = new LocalizedString() {{"en", $"Asset_Description_{rand}"}},
                Tags = new List<string> { $"Tag_{rand}_1", $"Tag_{rand}_2"}
            };
            return asset;
        }

        public static List<AssetSource> GetListOfAssetSource(int count = 2)
        {
            var assetSources = new List<AssetSource>();
            for (int i = 1; i<= count; i++)
            {
                int rand = RandomInt();
                assetSources.Add(GetAssetSource(rand));
            }
            return assetSources;
        }

        public static Dictionary<string, List<SearchKeywords>> GetSearchKeywords()
        {
            var searchKeywords = new Dictionary<string, List<SearchKeywords>>();
            var searchKeywordsList = GetSearchKeywordList();
            searchKeywords.Add("en", searchKeywordsList);
            return searchKeywords;
        }

        private static List<SearchKeywords> GetSearchKeywordList()
        {
            var searchKeywordsList = new List<SearchKeywords>
            {
                new SearchKeywords {Text = "Raider", SuggestTokenizer = GetCustomTokenizer()}
            };

            return searchKeywordsList;
        }

        private static SuggestTokenizer GetCustomTokenizer()
        {
            var suggestTokenizer = new CustomTokenizer();
            suggestTokenizer.Inputs = new List<string>{"Twix"};
            return suggestTokenizer;
        }
        private static AssetSource GetAssetSource(int random = 1)
        {
            var assetSource = new AssetSource
            {
                Key = $"AssetSource-Key-{random}",
                ContentType = "image/gif",
                Uri = $"http://www.commercetools.com/assets/img/logo_{random}.gif",
                Dimensions = new AssetDimensions{ H= 100, W = 100}
            };
            return assetSource;
        }

        #endregion
    }
}
