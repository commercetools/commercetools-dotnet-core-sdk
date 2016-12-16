using System;
using System.Collections.Generic;

using commercetools.Carts;
using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.TaxCategories
{
    /// <summary>
    /// TaxCategory
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-taxCategories.html#taxcategory"/>
    public class TaxCategory
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
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; private set; }

        [JsonProperty(PropertyName = "rates")]
        public List<TaxRate> Rates { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public TaxCategory(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Name = data.name;
            this.Description = data.description;
            this.Rates = Helper.GetListFromJsonArray<TaxRate>(data.rates);
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
            TaxCategory taxCategory = obj as TaxCategory;

            if (taxCategory == null)
            {
                return false;
            }

            return taxCategory.Id.Equals(this.Id);
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
