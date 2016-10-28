using System;

using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Categories
{
    /// <summary>
    /// API representation for creating a new category.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#categorydraft"/>
    public class CategoryDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; set; }

        [JsonProperty(PropertyName = "parent")]
        public Reference Parent { get; set; }

        [JsonProperty(PropertyName = "slug")]
        public LocalizedString Slug { get; set; }

        [JsonProperty(PropertyName = "orderHint")]
        public string OrderHint { get; set; }

        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; set; }

        [JsonProperty(PropertyName = "metaTitle")]
        public LocalizedString MetaTitle { get; set; }

        [JsonProperty(PropertyName = "metaDescription")]
        public LocalizedString MetaDescription { get; set; }

        [JsonProperty(PropertyName = "metaKeywords")]
        public LocalizedString MetaKeywords { get; set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFieldsDraft Custom { get; set; }


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name</param>
        public CategoryDraft(LocalizedString name, LocalizedString slug)
        {
            this.Name = name;
            this.Slug = slug;
        }

        #endregion
    }
}