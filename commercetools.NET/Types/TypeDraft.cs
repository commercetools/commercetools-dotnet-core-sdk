using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// TypeDrafts are given as payload for Create Type requests.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-types.html#typedraft"/>
    public class TypeDraft
    {
        #region Properties

        /// <summary>
        /// Identifier for the type (max. 256 characters).
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

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
        /// The IDs of the resources that can be customized with this type.
        /// </summary>
        [JsonProperty(PropertyName = "resourceTypeIds")]
        public List<string> ResourceTypeIds { get; set; }

        /// <summary>
        /// List of FieldDefinitions
        /// </summary>
        [JsonProperty(PropertyName = "fieldDefinitions")]
        public List<FieldDefinition> FieldDefinitions { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key">Identifier for the type (max. 256 characters).</param>
        /// <param name="name">Name</param>
        /// <param name="resourceTypeIds">The IDs of the resources that can be customized with this type.</param>
        public TypeDraft(string key, LocalizedString name, List<string> resourceTypeIds)
        {
            this.Key = key;
            this.Name = name;
            this.ResourceTypeIds = resourceTypeIds;
        }

        #endregion
    }
}
