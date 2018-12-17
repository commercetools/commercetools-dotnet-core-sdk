using System.Collections.Generic;

namespace commercetools.Sdk.Linq.Discount
{
    public static class DiscountMapping
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

        public static readonly Dictionary<string, string> PropertyAccessors = new Dictionary<string, string>()
        {
            { "ProductId", "product.id" },
            { "CustomerId", "customer.id" },
            { "TotalNet", "net" },
            { "TotalGross", "gross" }
        };

        public static readonly IEnumerable<string> AccessorsToSkip = new List<string>()
        {
            "Value",
            "Key"
        };

        public static readonly IEnumerable<string> CustomMethods = new List<string>()
        {
            "LineItemCount",
            "CustomLineItemCount",
            "LineItemTotal",
            "CustomLineItemTotal",
            "LineItemNetTotal",
            "CustomLineItemNetTotal",
            "LineItemGrossTotal",
            "CustomLineItemGrossTotal",
            "LineItemExists",
            "ForAllLineItems"
        };
    }
}