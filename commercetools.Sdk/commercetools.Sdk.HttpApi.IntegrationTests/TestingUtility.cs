using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Products.Attributes;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public static class TestingUtility
    {
        #region Fields

        private static readonly Random Random = new Random();

        private static readonly List<string> EuropeCountries = new List<string>()
            {"BE", "BG", "HR", "CY", "CZ", "DK", "EE", "FR", "HU", "IT", "IE", "NL", "PL", "PT", "ES", "SE"};

        #endregion

        #region Functions

        public static string RandomString(int length)
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
            var classificationValues = new List<LocalizedEnumValue>();
            classificationValues.Add(new LocalizedEnumValue()
                {Key = "Small", Label = new LocalizedString() {{"en", "Small"}, {"de", "Klein"}}});
            classificationValues.Add(new LocalizedEnumValue()
                {Key = "Medium", Label = new LocalizedString() {{"en", "Medium"}, {"de", "Mittel"}}});
            classificationValues.Add(new LocalizedEnumValue()
                {Key = "Heavy", Label = new LocalizedString() {{"en", "Heavy"}, {"de", "Schwergut"}}});
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

        public static PriceDraft GetPriceDraft(int centAmount,  DateTime validFrom, DateTime validUntil, string currency = "EUR")
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

        public static List<PriceDraft> GetRandomListOfPriceDraft(int count = 2)
        {
            List<PriceDraft> prices = new List<PriceDraft>();

            //first Add valid price for now
            prices.Add(GetPriceDraft(RandomInt(1000, 10000), DateTime.Now, DateTime.Now.AddMonths(1)));
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
                {Name = "localized-enum-attribute-name", Value = new LocalizedEnumValue() {Key = "enum-key-1"}});
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

        #endregion
    }
}
