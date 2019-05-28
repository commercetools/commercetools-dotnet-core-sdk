using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Common;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;
using LineItem = commercetools.Sdk.Domain.ShoppingLists.LineItem;

namespace commercetools.Sdk.Domain.Predicates
{
    public static class ExpansionExtensions
    {
        public static T ExpandAll<T>(this List<T> list)
        {
            return list.FirstOrDefault();
        }

        public static LineItem ExpandProductSlugs(this List<LineItem> list)
        {
            return list.FirstOrDefault();
        }

        public static Attribute ExpandValues(this List<Attribute> list)
        {
            return list.FirstOrDefault();
        }

        public static LineItem ExpandVariants(this List<LineItem> list)
        {
            return list.FirstOrDefault();
        }

        public static Reference<DiscountCode> ExpandDiscountCodes(this List<DiscountCodeInfo> list)
        {
            return list.FirstOrDefault().DiscountCode;
        }

        public static Reference<T> ExpandReferenceField<T>(this Dictionary<string, object> dictionary, string fieldName)
        {
            dictionary.TryGetValue(fieldName, out object value);
            return (Reference<T>)value;
        }

        public static Reference ExpandReferenceField(this Dictionary<string, object> dictionary, string fieldName)
        {
            dictionary.TryGetValue(fieldName, out object value);
            return (Reference)value;
        }
    }
}
