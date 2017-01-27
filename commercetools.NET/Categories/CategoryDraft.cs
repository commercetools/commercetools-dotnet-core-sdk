using System.Collections.Generic;

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

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; set; }

        /// <summary>
        /// Reference to a Category that is the parent of this category in the category tree.
        /// </summary>
        [JsonProperty(PropertyName = "parent")]
        public Reference Parent { get; set; }

        /// <summary>
        /// Human-readable identifier usually used as deep-link URL to the related category.
        /// </summary>
        /// <remarks>
        /// Allowed are alphabetic, numeric, underscore (_) and hyphen (-) characters. Maximum size is 256. Must be unique across a project! The same category can have the same slug for different languages.
        /// </remarks>
        [JsonProperty(PropertyName = "slug")]
        public LocalizedString Slug { get; set; }

        /// <summary>
        /// An attribute as base for a custom category order in one level.
        /// </summary>
        [JsonProperty(PropertyName = "orderHint")]
        public string OrderHint { get; set; }

        /// <summary>
        /// External Id
        /// </summary>
        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Meta title
        /// </summary>
        [JsonProperty(PropertyName = "metaTitle")]
        public LocalizedString MetaTitle { get; set; }

        /// <summary>
        /// Meta description
        /// </summary>
        [JsonProperty(PropertyName = "metaDescription")]
        public LocalizedString MetaDescription { get; set; }

        /// <summary>
        /// Meta keywords
        /// </summary>
        [JsonProperty(PropertyName = "metaKeywords")]
        public LocalizedString MetaKeywords { get; set; }

        /// <summary>
        /// The custom fields.
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public CustomFieldsDraft Custom { get; set; }

        /// <summary>
        /// List of AssetDrafts
        /// </summary>
        [JsonProperty(PropertyName = "assets")]
        public List<AssetDraft> Assets { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="slug">Human-readable identifier usually used as deep-link URL to the related category.</param>
        public CategoryDraft(LocalizedString name, LocalizedString slug)
        {
            this.Name = name;
            this.Slug = slug;
        }

        #endregion
    }
}
