using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Messages
{
    /// <summary>
    /// A message represents a change or an action performed on a resource (like an Order or a Product).
    /// </summary>
    [Endpoint("messages")]
    public abstract class Message: Resource<Message>
    {
        public int SequenceNumber { get; set; }

        public int ResourceVersion { get; set; }
        public Reference Resource { get; set; }

        public UserProvidedIdentifiers ResourceUserProvidedIdentifiers { get; set; }

        public virtual string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
            set {}
        }
    }
}
