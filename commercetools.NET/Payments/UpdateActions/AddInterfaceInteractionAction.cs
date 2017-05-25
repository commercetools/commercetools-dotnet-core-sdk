using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments.UpdateActions
{
    /// <summary>
    /// Adds a new interaction with the interface.
    /// </summary>
    /// <remarks>
    /// These can be notifications received from the PSP or requests send to the PSP. Some interactions may result in a transaction. If so, the interactionId in the transaction should be set to match the ID of the PSP for the interaction.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-payments.html#add-interfaceinteraction"/>
    public class AddInterfaceInteractionAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ResourceIdentifier to a Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public ResourceIdentifier Type { get; set; }

        /// <summary>
        /// CustomFields
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public CustomFields.CustomFields Fields { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">ResourceIdentifier to a Type</param>
        public AddInterfaceInteractionAction(ResourceIdentifier type)
        {
            this.Action = "addInterfaceInteraction";
            this.Type = type;
        }

        #endregion
    }
}
