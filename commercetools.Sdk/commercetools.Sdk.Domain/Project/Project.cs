using System;
using System.Collections;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Project
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
        public DateTime? CreatedAt { get; set; }
        public string TrialUntil { get; set; }
        public ShippingRateInputType ShippingRateInputType { get; set; }
    }
}
