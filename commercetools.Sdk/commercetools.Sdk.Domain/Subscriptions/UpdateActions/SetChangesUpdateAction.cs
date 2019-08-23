using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Subscriptions.UpdateActions
{
    public class SetChangesUpdateAction : UpdateAction<Subscription>
    {
        public string Action => "setKey";
        public List<ChangeSubscription> Changes { get; set; }
    }
}
