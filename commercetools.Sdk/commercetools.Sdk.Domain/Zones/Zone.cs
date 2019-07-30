using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Zones
{
    [Endpoint("zones")]
    [ResourceType(ReferenceTypeId.Zone)]
    public class Zone : Resource<Zone>, IKeyReferencable<Zone>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Location> Locations { get; set; }
    }
}
