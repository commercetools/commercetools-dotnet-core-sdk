using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Reviews;

namespace commercetools.Sdk.Domain.Channels
{
    /// <summary>
    /// Channels represent a source or destination of different entities. They can be used to model warehouses or stores.
    /// </summary>
    [Endpoint("channels")]
    [ResourceType(ReferenceTypeId.Channel)]
    public class Channel
    {
        public string Id { get; set; }

        public string Key { get; set; }

        public int Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public List<ChannelRole> Roles { get; set; }

        public LocalizedString Name { get; set; }

        public LocalizedString Description { get; set; }

        public Address Address { get; set; }

        public ReviewRatingStatistics ReviewRatingStatistics { get; set; }

        public CustomFields Custom { get; set; }
        public GeoJsonGeometry GeoLocation { get; set; }
    }
}
