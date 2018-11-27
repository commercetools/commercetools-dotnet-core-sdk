using System.Collections.Generic;

namespace commercetools.Sdk.Linq
{
    public static class Mappings
    {
        public static readonly Dictionary<string, string> MethodAccessors = new Dictionary<string, string>()
        {
            { "Currency", "currency" },
            { "Customer", "customer" },
            { "CustomerGroupKey", "customerGroup.key" },
            { "CustomTypeKey", "custom.type.key" },
            { "ProductKey", "product.key" },
            { "VariantId", "variantId" },
            { "CatalogId", "catalog.id" },
            { "DiscountId", "discount.id" },
            { "Sku", "sku" },
            { "CategoriesId", "categories.id" },
            { "CategoriesKey", "categories.key" },
            { "CategoriesWithAncestorsId", "categoriesWithAncestors.id" },
            { "CategoriesWithAncestorsKey", "categoriesWithAncestors.key" },
            { "Attributes", "attributes" }
        };
    }
}