using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Projects
{
    [Endpoint("")]
    public class Project
    {
        public int Version { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public IList<string> Countries { get; set; }
        public IList<string> Currencies { get; set; }
        public IList<string> Languages { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string TrialUntil { get; set; }
        public MessagesConfiguration Messages { get; set; }
        public ShippingRateInputType ShippingRateInputType { get; set; }
        public ExternalOAuth ExternalOAuth { get; set; }
    }
}
