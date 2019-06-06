using System;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class UpdateSyncInfoUpdateAction : UpdateAction<Order>
    {
        public string Action => "updateSyncInfo";
        [Required]
        public IReference<Channel> Channel { get; set; }
        public string ExternalId { get; set; }
        public DateTime? SyncedAt { get; set; }
    }
}
