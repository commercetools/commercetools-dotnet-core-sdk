using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// A message represents a change or an action performed on a resource.
    /// </summary>
    /// <remarks>
    /// Messages can be seen as a subset of the change history for a resource inside a project. It is a subset because not all changes on resources result in messages. This feature needs to be activated manually. 
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#message"/>
    public class Message
    {
        #region Properties

        /// <summary>
        /// The unique ID of the message.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected set; }

        /// <summary>
        /// Version
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public int? Version { get; protected set; }

        /// <summary>
        /// It's a number of this message in relation to other messages in the context of a specific resource.
        /// </summary>
        [JsonProperty(PropertyName = "sequenceNumber")]
        public string SequenceNumber { get; protected set; }

        /// <summary>
        /// A reference to the resource on which the change was performed.
        /// </summary>
        [JsonProperty(PropertyName = "resource")]
        public Reference Resource { get; protected set; }

        /// <summary>
        /// The version of the resource on which the change was performed.
        /// </summary>
        [JsonProperty(PropertyName = "resourceVersion")]
        public int? ResourceVersion { get; protected set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// Created At
        /// </summary>
        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; protected set; }

        /// <summary>
        /// Last Modified At
        /// </summary>
        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        protected Message(dynamic data)
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
