using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace commercetools.Sdk.Domain.Zones
{
    public class ZoneDraft : IDraft<Zone>
    {
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Location> Locations { get; set; }
    }
}
