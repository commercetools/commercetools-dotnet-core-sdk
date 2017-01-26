using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// Type
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-types.html#type"/>
    public class Type
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; private set; }

        [JsonProperty(PropertyName = "resourceTypeIds")]
        public List<string> ResourceTypeIds { get; private set; }

        [JsonProperty(PropertyName = "fieldDefinitions")]
        public List<FieldDefinition> FieldDefinitions { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Type(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.Key = data.key;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Name = new LocalizedString(data.name);
            this.Description = new LocalizedString(data.description);
            this.ResourceTypeIds = Helper.GetListFromJsonArray<string>(data.resourceTypeIds);
            this.FieldDefinitions = Helper.GetListFromJsonArray<FieldDefinition>(data.fieldDefinitions);
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
            Type type = obj as Type;

            if (type == null)
            {
                return false;
            }

            return type.Id.Equals(this.Id);
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
