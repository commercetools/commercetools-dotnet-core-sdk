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
        public string Id { get;}
        
        public int Version { get;}

        public int SequenceNumber { get;}
        
        public int ResourceVersion { get;}
        
        public DateTime CreatedAt { get;}

        public DateTime LastModifiedAt { get;}

        public ResourceIdentifier Resource { get;}

        public UserProvidedIdentifiers ResourceUserProvidedIdentifiers { get;}
            
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}
