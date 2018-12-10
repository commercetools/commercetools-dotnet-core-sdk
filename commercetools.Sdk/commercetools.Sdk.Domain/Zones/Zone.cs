using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Zones
{
    [Endpoint("zones")]
    public class Zone
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Key { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Location> Locations { get; set; }
    }
}
