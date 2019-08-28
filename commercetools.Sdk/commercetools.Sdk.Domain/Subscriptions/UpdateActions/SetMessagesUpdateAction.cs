using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Subscriptions.UpdateActions
{
    public class SetMessagesUpdateAction : UpdateAction<Subscription>
    {
        public string Action => "setMessages";
        public List<MessageSubscription> Messages { get; set; }
    }
}
