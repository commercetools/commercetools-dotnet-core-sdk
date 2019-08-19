using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.IntegrationTests
{
    public static class Extensions
    {
        public static double NextDouble(this Random rnd, double min, double max)
        {
            return rnd.NextDouble() * (max-min) + min;
        }

        public static string GetTextAttributeValue(this ProductVariant variant, string textAttributeName)
        {
            string attributeValue = null;
            var attribute = variant.Attributes.FirstOrDefault(a => a.Name.Equals(textAttributeName));
            if (attribute != null)//if there is attribute with name = textAttributeName
            {
                attributeValue = (attribute as Attribute<string>)?.Value;
            }
            return attributeValue;
        }

        public static Money ToMoney(this BaseMoney baseMoney)
        {
            var money = new Money
            {
                Type = baseMoney.Type,
                CentAmount = baseMoney.CentAmount,
                CurrencyCode = baseMoney.CurrencyCode,
                FractionDigits = baseMoney.FractionDigits
            };
            return money;
        }
        public static bool DictionaryEqual<TKey, TValue>(
            this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            return first.DictionaryEqual(second, null);
        }

        public static bool DictionaryEqual<TKey, TValue>(
            this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second,
            IEqualityComparer<TValue> valueComparer)
        {
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;

            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            foreach (var kvp in first)
            {
                if (!second.TryGetValue(kvp.Key, out var secondValue)) return false;
                if (!valueComparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }
    }
}
