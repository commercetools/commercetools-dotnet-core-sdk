using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Categories
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CategoryDraft : IDraft<Category>
    {
        public string Key { get; set; }

        [Required]
        public LocalizedString Name { get; set; }

        public LocalizedString Description { get; set; }

        public IReference<Category> Parent { get; set; }

        [Required]
        public LocalizedString Slug { get; set; }

        public string OrderHint { get; set; }

        public string ExternalId { get; set; }

        public LocalizedString MetaTitle { get; set; }

        public LocalizedString MetaDescription { get; set; }

        public LocalizedString MetaKeywords { get; set; }

        public CustomFieldsDraft Custom { get; set; }

        public List<AssetDraft> Assets { get; set; }
    }
}
