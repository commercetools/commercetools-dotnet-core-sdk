using System;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class UpdateSyncInfoStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "updateSyncInfo";
        [Required]
        public IReference<Channel> Channel { get; set; }
        public string ExternalId { get; set; }
        public DateTime? SyncedAt { get; set; }
    }
}
