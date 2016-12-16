using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// Product types are used to describe common characteristics, most importantly common custom attributes, of many concrete products.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#producttype"/>
    public class ProductType
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

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; private set; }

        [JsonProperty(PropertyName = "attributes")]
        public List<AttributeDefinition> Attributes { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductType(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Key = data.key;
            this.Name = data.name;
            this.Description = data.description;
            this.Attributes = Helper.GetListFromJsonArray<AttributeDefinition>(data.attributes);
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
            ProductType product = obj as ProductType;

            if (product == null)
            {
                return false;
            }

            return product.Id.Equals(this.Id);
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
