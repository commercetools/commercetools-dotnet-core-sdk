using System.Collections.Generic;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Messages.Stores
{
    [TypeMarker("StoreCreated")]
    public class StoreCreatedMessage : Message<Store>
    {
        public LocalizedString Name { get; set; }
        public List<string> Languages { get; set; }
        public List<IReference<Channel>> DistributionChannels { get; set; }
        public List<IReference<Channel>> SupplyChannels { get; set; }
        public CustomFields Custom { get; set; }
    }
}
