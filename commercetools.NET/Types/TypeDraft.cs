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

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; set; }

        [JsonProperty(PropertyName = "resourceTypeIds")]
        public List<string> ResourceTypeIds { get; set; }

        [JsonProperty(PropertyName = "fieldDefinitions")]
        public List<FieldDefinition> FieldDefinitions { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="currency">Currency</param>
        public TypeDraft(string key, LocalizedString name, List<string> resourceTypeIds)
        {
            this.Key = key;
            this.Name = name;
            this.ResourceTypeIds = resourceTypeIds;
        }

        #endregion
    }
}
