using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Subscriptions.UpdateActions
{
    /// <summary>
    /// Set Messages
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#set-messages"/>
    public class SetMessagesAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Messages
        /// </summary>
        [JsonProperty(PropertyName = "messages")]
        public List<MessageSubscription> Messages { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetMessagesAction()
        {
            this.Action = "setMessages";
        }

        #endregion
    }
}
