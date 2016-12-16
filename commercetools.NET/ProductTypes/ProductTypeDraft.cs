using System.Collections.Generic;

using Newtonsoft.Json;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// The representation to be sent to the server when creating a new ProductType.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#producttypedraft"/>
    public class ProductTypeDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "attributes")]
        public List<AttributeDefinitionDraft> Attributes { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        public ProductTypeDraft(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        #endregion
    }
}
