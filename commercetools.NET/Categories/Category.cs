using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Categories
{
    /// <summary>
    /// Categories are used to organize products in a hierarchical structure.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#category"/>
    public class Category
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        [JsonProperty(PropertyName = "slug")]
        public LocalizedString Slug { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; private set; }

        [JsonProperty(PropertyName = "ancestors")]
        public List<Reference> Ancestors { get; private set; }

        [JsonProperty(PropertyName = "parent")]
        public Reference Parent { get; private set; }

        [JsonProperty(PropertyName = "orderHint")]
        public string OrderHint { get; private set; }

        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; private set; }

        [JsonProperty(PropertyName = "metaTitle")]
        public LocalizedString MetaTitle { get; private set; }

        [JsonProperty(PropertyName = "metaDescription")]
        public LocalizedString MetaDescription { get; private set; }

        [JsonProperty(PropertyName = "metaKeywords")]
        public LocalizedString MetaKeywords { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Category(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Name = new LocalizedString(data.name);
            this.Slug = new LocalizedString(data.slug);
            this.Description = new LocalizedString(data.description);
            this.Ancestors = Helper.GetListFromJsonArray<Reference>(data.ancestors);
            this.Parent = new Reference(data.parent);
            this.OrderHint = data.orderHint;
            this.ExternalId = data.externalId;
            this.MetaTitle = new LocalizedString(data.metaTitle);
            this.MetaDescription = new LocalizedString(data.metaDescription);
            this.MetaKeywords = new LocalizedString(data.metaKeywords);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Category category = obj as Category;

            if (category == null)
            {
                return false;
            }

            return category.Id.Equals(this.Id);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }    

        #endregion
    }
}
