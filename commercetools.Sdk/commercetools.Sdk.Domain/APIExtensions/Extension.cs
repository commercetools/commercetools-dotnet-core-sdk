using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.APIExtensions
{
    [Endpoint("extensions")]
    public class Extension : Resource<Extension>, IKeyReferencable<Extension>
    {
        public string Key { get; }

        public Destination Destination { get; set; }

        public List<Trigger> Triggers { get; set; }

        public long TimeoutInMs { get; set; }
    }
}
