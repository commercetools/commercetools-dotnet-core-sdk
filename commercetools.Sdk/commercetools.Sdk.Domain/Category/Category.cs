namespace commercetools.Sdk.Domain
{
    using System;
    using System.Collections.Generic;

    [Endpoint("categories")]
    public class Category
    {
        public string Id { get; set; }

        public string Key { get; set; }

        public int Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public LocalizedString Name { get; set; }

        public LocalizedString Slug { get; set; }

        public LocalizedString Description { get; set; }

        public List<Reference<Category>> Ancestors { get; set; }

        public Reference<Category> Parent { get; set; }

        public string OrderHint { get; set; }

        public string ExternalId { get; set; }

        public LocalizedString MetaTitle { get; set; }

        public LocalizedString MetaDescription { get; set; }

        public LocalizedString MetaKeywords { get; set; }

        public CustomFields Custom { get; set; }

        public List<Asset> Assets { get; set; }
    }
}