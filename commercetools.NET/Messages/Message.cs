using System;

using commercetools.Common;
using commercetools.Customers;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// A message represents a change or an action performed on a resource.
    /// </summary>
    /// <remarks>
    /// Messages can be seen as a subset of the change history for a resource inside a project. It is a subset because not all changes on resources result in messages. This feature needs to be activated manually. 
    /// </remarks>
    public class Message
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected set; }

        [JsonProperty(PropertyName = "version")]
        public int? Version { get; protected set; }

        [JsonProperty(PropertyName = "sequenceNumber")]
        public string SequenceNumber { get; protected set; }

        [JsonProperty(PropertyName = "resource")]
        public Reference Resource { get; protected set; }

        [JsonProperty(PropertyName = "resourceVersion")]
        public int? ResourceVersion { get; protected set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; protected set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        protected Message(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.SequenceNumber = data.sequenceNumber;
            this.Resource = new Reference(data.resource);
            this.ResourceVersion = data.resourceVersion;
            this.Type = data.type;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
        }

        #endregion
    }
}