using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Messages
{
    /// <summary>
    /// A message represents a change or an action performed on a resource (like an Order or a Product).
    /// </summary>
    [Endpoint("messages")]
    public abstract class Message
    {
        public string Id { get; set; }

        public int Version { get; set; }

        public int SequenceNumber { get; set; }

        public int ResourceVersion { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public ResourceIdentifier Resource { get; set; }

        public UserProvidedIdentifiers ResourceUserProvidedIdentifiers { get; set; }

        public virtual string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
            set { }
        }
    }
}
