namespace commercetools.Sdk.Domain
{
    using System;
    using System.Collections.Generic;

    public class CategoryDraft : IDraft<Category>
    {
        public string Key { get; set; }

        public LocalizedString Name { get; set; }

        public LocalizedString Description { get; set; }

        public Reference Parent { get; set; }

        public LocalizedString Slug { get; set; }

        public string OrderHint { get; set; }

        public string ExternalId { get; set; }

        public LocalizedString MetaTitle { get; set; }

        public LocalizedString MetaDescription { get; set; }

        public LocalizedString MetaKeywords { get; set; }

        public CustomFields Custom { get; set; }

        public List<Asset> Assets { get; set; }
    }
}