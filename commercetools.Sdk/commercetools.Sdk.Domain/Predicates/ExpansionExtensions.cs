using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain.ShoppingLists;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

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
    }
}
