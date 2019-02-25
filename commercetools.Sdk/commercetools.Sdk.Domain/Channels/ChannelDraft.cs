using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Channels
{
    public class ChannelDraft : IDraft<Channel>
    {
        [Required]
        public string Key { get; set; }

        public List<ChannelRole> Roles { get; set; }
        
        public LocalizedString Name { get; set; }

        public LocalizedString Description { get; set; }
        
        public Address Address { get; set; }
        
        public CustomFieldsDraft Custom { get; set; }
        
        public GeoJsonGeometry GeoLocation { get; set; }
    }
}